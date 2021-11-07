using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace RTIO
{
    internal partial class WFAdapater : Form, IWindow, IDisposable
    {
        PictureBox outPictureBox = null;  // Será liberado somente pelo GC

        public event Action<TimeSpan> OnUpdate;
        public new event Action<Key> OnKeyDown;
        public new event Action<Key> OnKeyUp;
        public new event Action<(int x, int y)> OnMouseDown;
        public new event Action<(int x, int y)> OnMouseUp;
        public event Action OnFocus;
        public event Action OnFocusOut;

        public WFAdapater(System.Drawing.Image source)
        {
            InitializeComponent();

            MouseDown += (_, args) => { OnMouseDown?.Invoke((args.X, args.Y)); };
            MouseUp += (_, args) => { OnMouseUp?.Invoke((args.X, args.Y)); };
            KeyDown += (_, args) => { OnKeyDown?.Invoke((Key)args.KeyCode); };
            KeyUp += (_, args) => { OnKeyUp?.Invoke((Key)args.KeyCode); };
            outPictureBox.Click += (_, _) => { OnFocus?.Invoke(); };
            LostFocus += (_, _) => { OnFocusOut?.Invoke(); };

            Stopwatch rePaintStopwatch = new Stopwatch();
            outPictureBox.Paint += (_, _) =>
            {
                rePaintStopwatch.Restart();
                OnUpdate?.Invoke(rePaintStopwatch.Elapsed);
                outPictureBox.Image = source;
            };
        }

        bool maximized = false;
        public bool Maximized
        {
            get => maximized;
            set
            {
                maximized = value;

                if (value)
                {
                    FormBorderStyle = FormBorderStyle.None;
                    WindowState = FormWindowState.Maximized;
                }
                else
                {
                    FormBorderStyle = FormBorderStyle.FixedSingle;
                    WindowState = FormWindowState.Normal;
                    Dimensions = (Width, Height);
                }
            }
        }

        public (int width, int height) Dimensions
        {
            get => (ClientSize.Width, ClientSize.Height);
            set
            {
                Size size = new Size(value.width, value.height);
                ClientSize = size;
            }
        }
    }
}
