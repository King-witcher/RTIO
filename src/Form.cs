using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace RTIO
{
    partial class Form : System.Windows.Forms.Form, IDisposable
    {
        PictureBox outPictureBox = null;  // Será liberado somente pelo GC

        #region Events

        public event Action<TimeSpan> OnUpdate;

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

        public Form(System.Drawing.Image source)
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

        public (int Width, int Height) Dimensions
        {
            get => (ClientSize.Width, ClientSize.Height);
            set
            {
                Size size = new Size(value.Width, value.Height);
                ClientSize = size;
            }
        }
    }
}
