using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace RTIO
{
    // Estou usando um Wrapper para livrar o usuário final de todos os métodos e propriedades herdadas pelo Form.
    public partial class Window : IWindow, IDisposable
    {
        WFAdapater window;
        System.Drawing.Image source;

        public event Action<TimeSpan> OnUpdate;
        public event Action<Key> OnKeyDown;
        public event Action<Key> OnKeyUp;
        public event Action<(int x, int y)> OnMouseDown;
        public event Action<(int x, int y)> OnMouseUp;
        public event Action OnFocus;
        public event Action OnFocusOut;

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

            // Setup events
            window.OnUpdate += OnUpdate;
            window.OnKeyDown += OnKeyDown;
            window.OnKeyUp += OnKeyUp;
            window.OnMouseDown += OnMouseDown;
            window.OnMouseUp += OnMouseUp;
            window.OnFocus += OnFocus;
            window.OnFocusOut += OnFocusOut;
        }

        public void Start()
        {
            OnFocus?.Invoke();
            Application.Run(window);
        }

        public void Dispose()
        {
            window.Dispose();
            source.Dispose();
        }
    }
}
