using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace RTIO
{
    /// <summary>
    /// Representa uma imagem representada por um buffer com 32 bits por pixel.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct Image : IDisposable
    {
        [FieldOffset(0)] int width;
        [FieldOffset(4)] int height;

        // Union
        [FieldOffset(16)] uint* uint0;
        [FieldOffset(16)] Color* rgb0;

        /// <summary>
        /// Obtém ou define a cor de um pixel com base em suas coordenadas.
        /// </summary>
        /// <param name="column">A coordenada horizontal</param>
        /// <param name="line">A coordenada vertical</param>
        /// <returns></returns>
        public Color this[int column, int line]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => rgb0[column + width * line];
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => rgb0[column + width * line] = value;
        }

        /// <summary>
        /// Obtém a altura da imagem, em pixels.
        /// </summary>
        public int Height => height;

        /// <summary>
        /// Obtém o comprimento da imagem, em pixels.
        /// </summary>
        public int Width => width;

        /// <summary>
        /// Obtém um ponteiro em contexto seguro para o primeiro pixel do buffer.
        /// </summary>
        public IntPtr Scan0 => (IntPtr)uint0;

        /// <summary>
        /// Obtém um ponteiro para o primeiro pixel do buffer.
        /// </summary>
        public Color* RGB0 => rgb0;

        /// <summary>
        /// Obtém um ponteiro para o primeiro pixel do buffer, tratado como uint32.
        /// </summary>
        public uint* Uint0 => uint0;

        /// <summary>
        /// Obtém o System.Drawing.Imaging.PixelFormat correspondente ao padrão utilizado pela classe.
        /// </summary>
        public static PixelFormat PixelFormat => PixelFormat.Format32bppArgb;

        // Descrição sem certeza, preciso rever depois
        /// <summary>
        /// Cria uma instância de Image clonando uma System.Drawing.Bitmap.
        /// </summary>
        /// <param name="source">Bitmap a ser clonada</param>
        /// <exception cref="ArgumentNullException">A Bitmap não pode ser nula.</exception>
        public Image(Bitmap source)
        {
            Rectangle rect = new(0, 0, source.Width, source.Height);
            using (var clone = source.Clone(rect, PixelFormat.Format32bppArgb) ??
                throw new ArgumentNullException("Bitmap parameter cannot be null."))
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

        /// <summary>
        /// Obtém uma nova imagem cmo dimensões especificadas, podendo esta conter lixo de memória de onde seu buffer for alocado
        /// </summary>
        /// <param name="width">O comprimento da imagem</param>
        /// <param name="height">A altura da imagem</param>
        /// <exception cref="ArgumentOutOfRangeException">Lançada caso as dimensões passadas sejam negativas</exception>
        public Image(int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentOutOfRangeException();

            this.width = width;
            this.height = height;
            rgb0 = null; // Assigned by union
            uint0 = (uint*)Marshal.AllocHGlobal(width * height * sizeof(uint));
        }

        /// <summary>
        /// Faz com que esta instância de Image clone os dados de outra.
        /// </summary>
        /// <remarks>Ambas as instâncias de Image precisam ter dimensões idênticas.</remarks>
        /// <param name="source">Imagem a ser clonada</param>
        /// <exception cref="ArgumentException">Lançada caso as imagens tenham dimensões diferentes.</exception>
        public void Clone(Image source)
        {
            if (width != source.width || height != source.height)
                throw new ArgumentException("Buffers must have the same size.");
            System.Buffer.MemoryCopy(source.uint0, this.uint0, 4 * height * width, 4 * height * width);
        }

        /// <summary>
        /// Executa uma função f: Color -> Color em todos os pixels da imagem em multithread.
        /// </summary>
        /// <param name="filter"></param>
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
                    buffer[cur] = (uint)filter(buffer[cur]);
                }
            });
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(Scan0);
        }

        /// <summary>
        /// Clona os dados de uma Bitmap e cria uma Image equivalente.
        /// </summary>
        /// <remarks>Obsoleto, não use.</remarks>
        /// <param name="bitmap">Bitmap a ser clonado</param>
        [Obsolete]
        public static explicit operator Image(Bitmap bitmap)
        {
            return new Image(bitmap);
        }

        /// <summary>
        /// Converte a instância atual de Image em uma Bitmap, reutilizando dados não gerenciados.
        /// </summary>
        /// <param name="texture">Image a ser convertida em Bitmap.</param>
        public static explicit operator Bitmap(Image texture)
        {
            return new Bitmap(texture.Width, texture.Height, 4 * texture.Width, Image.PixelFormat, texture.Scan0);
        }
    }
}
