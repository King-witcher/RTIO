using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTIO
{
    // Estou usando um Wrapper para livrar o usuário final de todos os métodos e propriedades herdadas pelo Form.
    /// <summary>
    /// Representa uma janela capaz de exibir uma imagem em tempo real.
    /// </summary>
    public partial class Window : IWindow, IDisposable
    {
        WFAdapater window;
        Bitmap source;

        #region Events

        public event Action<TimeSpan> OnUpdate
        {
            add => window.OnUpdate += value;
            remove => window.OnUpdate -= value;
        }

        public event Action<Key> OnKeyDown
        {
            add => window.OnKeyDown += value;
            remove => window.OnKeyDown -= value;
        }

        public event Action<Key> OnKeyUp
        {
            add => window.OnKeyUp += value;
            remove => window.OnKeyUp -= value;
        }

        public event Action<(int x, int y)> OnMouseDown
        {
            add => window.OnMouseDown += value;
            remove => window.OnMouseDown -= value;
        }

        public event Action<(int x, int y)> OnMouseUp
        {
            add => window.OnMouseUp += value;
            remove => window.OnMouseUp -= value;
        }

        public event Action OnFocus
        {
            add => window.OnFocus += value;
            remove => window.OnFocus -= value;
        }

        public event Action OnFocusOut
        {
            add => window.OnFocusOut += value;
            remove => window.OnFocusOut -= value;
        }

        #endregion

        /// <summary>
        /// Obtém ou define se a janela será maximizada.
        /// </summary>
        public bool Maximized
        {
            get => window.Maximized;
            set => window.Maximized = value;
        }

        /// <summary>
        /// Obtém ou define as dimensões internas da janela.
        /// </summary>
        public (int width, int height) Dimensions
        {
            get => window.Dimensions;
            set => window.Dimensions = value;
        }

        /// <summary>
        /// Obtém uma nova instância de Window.
        /// </summary>
        /// <param name="output">Imagem a ser exibida na tela</param>
        public unsafe Window(Image output)
        {
            // Setup a bitmap instance that points to the given output buffer
            source = new(
                output.Width, output.Height,
                output.Width * sizeof(uint), PixelFormat.Format32bppRgb,
                (IntPtr)output.Uint0);

            window = new WFAdapater(source);

            Dimensions = (output.Width, output.Height);
        }

        /// <summary>
        /// Abre a janela e toma o controle da thread atual.
        /// </summary>
        /// <remaks>
        /// Window.Start() precisa estar funcionando na thread principal para que a janela seja exibida corretamente.
        /// </remaks>
        public async void Start()
        {
            Task.Run(() => Application.Run(window)).Wait();
        }

        public void Dispose()
        {
            window.Dispose();
            source.Dispose();
        }
    }
}
