using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTIO
{
    // Estou usando um Wrapper para livrar o usuário final de todos os métodos e propriedades herdadas pelo Form.
    public partial class Window : IWindow, IDisposable
    {
        WFAdapater window;
        System.Drawing.Image source;

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

        public bool Maximized
        {
            get => window.Maximized;
            set => window.Maximized = value;
        }

        public (int width, int height) Dimensions
        {
            get => window.Dimensions;
            set => window.Dimensions = value;
        }

        public unsafe Window(Image output)
        {
            // Setup a bitmap instance that points to the given output buffer
            source = new Bitmap(
                output.Width, output.Height,
                output.Width * sizeof(uint), PixelFormat.Format32bppRgb,
                (IntPtr)output.Uint0);

            window = new WFAdapater(source);

            Dimensions = (output.Width, output.Height);
        }

        public async void Start()
        {
            Task.Run(() => Application.Run(window)).Wait();
            Task.Run(() => Application.Run(window)).Wait();
        }

        public void Dispose()
        {
            window.Dispose();
            source.Dispose();
        }
    }
}
