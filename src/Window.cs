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
    public partial class Window : BaseWindow, IDisposable
    {
        Form wrappedForm;
        Bitmap source;

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

            wrappedForm = new Form(source);

            Dimensions = (output.Width, output.Height);
        }

        #region Events

        /// <inheritdoc/>
        public override event Action<TimeSpan> OnUpdate
        {
            add => wrappedForm.OnUpdate += value;
            remove => wrappedForm.OnUpdate -= value;
        }

        /// <inheritdoc/>
        public override event Action<Key> OnKeyDown
        {
            add => wrappedForm.KeyDown += (_, args) => value((Key) args.KeyCode);
            remove => wrappedForm.KeyDown -= (_, args) => value((Key)args.KeyCode);
        }

        /// <inheritdoc/>
        public override event Action<Key> OnKeyUp
        {
            add => wrappedForm.KeyUp += (_, args) => value((Key)args.KeyCode);
            remove => wrappedForm.KeyUp -= (_, args) => value((Key)args.KeyCode);
        }

        /// <inheritdoc/>
        public override event Action<(int, int)> OnMouseDown
        {
            add => wrappedForm.MouseDown += (_, args) => value((args.X, args.Y));
            remove => wrappedForm.MouseDown -= (_, args) => value((args.X, args.Y));
        }

        /// <inheritdoc/>
        public override event Action<(int, int)> OnMouseUp
        {
            add => wrappedForm.MouseUp += (_, args) => value((args.X, args.Y));
            remove => wrappedForm.MouseUp -= (_, args) => value((args.X, args.Y));
        }

        /// <inheritdoc/>
        public override event Action OnFocus
        {
            add => wrappedForm.OnFocus += value;
            remove => wrappedForm.OnFocus -= value;
        }

        /// <inheritdoc/>
        public override event Action OnFocusOut
        {
            add => wrappedForm.OnFocusOut += value;
            remove => wrappedForm.OnFocusOut -= value;
        }

        #endregion

        /// <inheritdoc/>
        public override bool Maximized
        {
            get => wrappedForm.Maximized;
            set => wrappedForm.Maximized = value;
        }

        /// <inheritdoc/>
        public override (int, int) Dimensions
        {
            get => wrappedForm.Dimensions;
            set => wrappedForm.Dimensions = value;
        }

        /// <inheritdoc/>
        public override void Run()
        {
            Task.Run(() => Application.Run(wrappedForm)).Wait();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            wrappedForm.Dispose();
            source.Dispose();
        }
    }
}
