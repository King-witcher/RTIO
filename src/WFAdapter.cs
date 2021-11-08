using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace RTIO
{
    partial class WFAdapater : Form, IWindow, IDisposable
    {
        PictureBox outPictureBox = null;  // Será liberado somente pelo GC

        #region Events

        public event Action<TimeSpan> OnUpdate;

        public new event Action<Key> OnKeyDown
        {
            add => KeyDown += (_, args) => { value((Key)args.KeyCode); };
            remove => KeyDown -= (_, args) => { value((Key)args.KeyCode); };
        }

        public new event Action<Key> OnKeyUp
        {
            add => KeyUp += (_, args) => { value((Key)args.KeyCode); };
            remove => KeyUp -= (_, args) => { value((Key)args.KeyCode); };
        }

        public new event Action<(int x, int y)> OnMouseDown
        {
            add => MouseDown += (_, args) => { value((args.X, args.Y)); };
            remove => MouseDown -= (_, args) => { value((args.X, args.Y)); };
        }

        public new event Action<(int x, int y)> OnMouseUp
        {
            add => MouseUp += (_, args) => { value((args.X, args.Y)); };
            remove => MouseUp -= (_, args) => { value((args.X, args.Y)); };
        }

        public event Action OnFocus
        {
            add => outPictureBox.Click += (_, _) => value();
            remove => outPictureBox.Click -= (_, _) => value();
        }

        public event Action OnFocusOut
        {
            add => LostFocus += (_, _) => value();
            remove => LostFocus -= (_, _) => value();
        }

        #endregion

        public WFAdapater(System.Drawing.Image source)
        {
            InitializeComponent();

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
