using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace RTIO
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe readonly struct Image : IDisposable
    {
        [FieldOffset(0)]
        readonly int width;
        [FieldOffset(4)]
        readonly int height;

        // Union
        [FieldOffset(16)]
        readonly uint* uint0;
        [FieldOffset(16)]
        readonly Color* rgb0;

        public Color this[int column, int line]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => rgb0[column + width * line];
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => rgb0[column + width * line] = value;
        }

        public int Height => height;
        public int Width => width;
        public IntPtr Scan0 => (IntPtr)uint0;
        public Color* RGB0 => rgb0;
        public uint* Uint0 => uint0;
        public PixelFormat PixelFormat => PixelFormat.Format32bppArgb;

        public Image(Bitmap source)
        {
            Rectangle rect = new Rectangle(0, 0, source.Width, source.Height);
            using (var clone = source.Clone(rect, PixelFormat.Format32bppArgb) ??
                throw new ArgumentException("Bitmap parameter cannot be null."))
            {
                var bmpdata = clone.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                int bmpsize = bmpdata.Stride * bmpdata.Height;
                rgb0 = null; // Assigned next line
                uint0 = (UInt32*)Marshal.AllocHGlobal(bmpsize);
                System.Buffer.MemoryCopy((void*)bmpdata.Scan0, uint0, bmpsize, bmpsize);
                clone.UnlockBits(bmpdata);
            }
            width = source.Width;
            height = source.Height;
        }

        public Image(int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentOutOfRangeException();

            this.width = width;
            this.height = height;
            rgb0 = null; // Assigned by union
            uint0 = (uint*)Marshal.AllocHGlobal(width * height * sizeof(uint));
        }

        public void Clone(Image source)
        {
            if (width != source.width || height != source.height)
                throw new ArgumentException("Buffers must have the same size.");
            System.Buffer.MemoryCopy(source.uint0, this.uint0, 4 * height * width, 4 * height * width);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Foreach(Func<Color, Color> filter)
        {
            int height = this.height;
            int width = this.width;
            uint* buffer = this.uint0;

            Parallel.For(0, width, (x) =>
            {
                for (int y = 0; y < height; y++)
                {
                    int cur = width * y + x;
                    buffer[cur] = filter(buffer[cur]);
                }
            });
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(Scan0);
        }

        public static explicit operator Image(Bitmap bitmap)
        {
            return new Image(bitmap);
        }

        public static explicit operator Bitmap(Image texture)
        {
            return new Bitmap(texture.Width, texture.Height, 4 * texture.Width, texture.PixelFormat, texture.Scan0);
        }
    }
}
