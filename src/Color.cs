using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RTIO
{
    /// <summary>
    /// Representa uma cor RGB.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct Color
    {
        [FieldOffset(0)] uint rgb;

        [FieldOffset(0)] byte b;
        [FieldOffset(1)] byte g;
        [FieldOffset(2)] byte r;
        [FieldOffset(3)] byte a;

        /// <summary>
        /// Cria uma nova instância de Color.
        /// </summary>
        /// <param name="red">A componente vermelha, de 0 a 255.</param>
        /// <param name="green">A componente verde, de 0 a 255.</param>
        /// <param name="blue">A componente azul, de 0 a 255.</param>
        public Color(byte red, byte green, byte blue)
        {
            rgb = 0;
            a = 0xff;
            r = red;
            g = green;
            b = blue;
        }

        /// <summary>
        /// Obém o brilho percebido da cor atual, de 0f a 1f.
        /// </summary>
        public float Luma => (0.2126f * r * r + 0.7152f * g * g + 0.0722f * b * b) / (255f * 255f);

        /// <summary>
        /// Obém o brilho percebido da cor atual, de 0f a 1f, calculado de forma mais rápida.
        /// </summary>
        public float FastLuma => (0.2126f * r + 0.7152f * g + 0.0722f * b) / (255f);

        /// <summary>
        /// Obtém o valor médio das componentes da cor.
        /// </summary>
        public byte Brightness => (byte)((r + g + b) / 3);

        /// <summary>
        /// Componente vermelha
        /// </summary>
        public byte R => r;

        /// <summary>
        /// Componente verde
        /// </summary>
        public byte G => g;

        /// <summary>
        /// Componente azul
        /// </summary>
        public byte B => b;

        /// <summary>
        /// Obtém a média entre esta e outra cor.
        /// </summary>
        /// <param name="rgb"></param>
        /// <returns>A média entre as duas cores</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color Average(Color rgb)
        {
            rgb.r = (byte)((r + rgb.r) >> 1);
            rgb.g = (byte)((g + rgb.g) >> 1);
            rgb.b = (byte)((b + rgb.b) >> 1);

            return rgb;
        }

        /// <summary>
        /// Obtém uma média ponderada entre esta e outra cor.
        /// </summary>
        /// <param name="rgb">Cor a ser acrescentada</param>
        /// <param name="weight">Peso da cor a ser acrescentada</param>
        /// <returns>A média ponderada entre as duas cores</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color Mix(Color rgb, float weight)
        {
            ushort parcel1, parcel2;

            parcel1 = (ushort)(r * (1 - weight));
            parcel2 = (ushort)(rgb.r * weight);
            rgb.r = (byte)(parcel1 + parcel2);

            parcel1 = (ushort)(g * (1 - weight));
            parcel2 = (ushort)(rgb.g * weight);
            rgb.g = (byte)(parcel1 + parcel2);

            parcel1 = (ushort)(b * (1 - weight));
            parcel2 = (ushort)(rgb.b * weight);
            rgb.b = (byte)(parcel1 + parcel2);

            return rgb;
        }

        /// <summary>
        /// Converte uma cor em um uint.
        /// </summary>
        /// <param name="color"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator uint(Color color) => color.rgb;

        /// <summary>
        /// Converte implicitamente um uint em uma cor.
        /// </summary>
        /// <param name="color"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Color(uint color) => new (){ rgb = color };

        /// <summary>
        /// Converte implicitamente uma tupla de três inteiros entre 0 e 255 em uma cor.
        /// </summary>
        /// <param name="components">As três componentes de uma cor</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Color((int r, int g, int b) components)
        {
            return new((byte)components.r, (byte)components.g, (byte)components.b);
        }

        /// <summary>
        /// Converte implicitamente entre System.Drawing.Color e RTIO.Color.
        /// </summary>
        /// <param name="color"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Color(System.Drawing.Color color)
        {
            return new(color.R, color.G, color.B);
        }

        /// <summary>
        /// Converte implicitamente entre System.Drawing.Color e RTIO.Color.
        /// </summary>
        /// <param name="color"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator System.Drawing.Color(Color color)
        {
            return System.Drawing.Color.FromArgb(255, color.r, color.g, color.b);
        }
    }
}
