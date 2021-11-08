using System;

namespace RTIO
{
    // Fiz isso porque quero implmentar outras formas de Window, futuramente. Possivelmente, usando WPF e comparar a performance.
    /// <summary>
    /// Representa uma janela capaz de exibir uma imagem em tempo real.
    /// </summary>
    public abstract class BaseWindow
    {
        /// <summary>
        /// Ocorre sempre que a imagem da janela é atualizada.
        /// </summary>
        public abstract event Action<TimeSpan> OnUpdate;

        /// <summary>
        /// Ocorre quando uma tecla é pressionada.
        /// </summary>
        public abstract event Action<Key> OnKeyDown;

        /// <summary>
        /// Ocorre quando uma tecla é solta.
        /// </summary>
        public abstract event Action<Key> OnKeyUp;

        /// <summary>
        /// Ocorre quando o botão do mouse é pressionado.
        /// </summary>
        public abstract event Action<(int X, int Y)> OnMouseDown;

        /// <summary>
        /// Ocorre quando o botão do mouse é solto.
        /// </summary>
        public abstract event Action<(int X, int Y)> OnMouseUp;

        /// <summary>
        /// Ocorre quando a janela recebe foco do cliente.
        /// </summary>
        public abstract event Action OnFocus;

        /// <summary>
        /// Ocorre quando a janela perde o foco do cliente.
        /// </summary>
        public abstract event Action OnFocusOut;

        /// <summary>
        /// Obtém ou define as dimensões internas da janela.
        /// </summary>
        public abstract (int Width, int Height) Dimensions { get; set; }

        /// <summary>
        /// Obtém ou define se a janela será maximizada.
        /// </summary>
        public abstract bool Maximized { get; set; }

        /// <summary>
        /// Abre a janela e toma o controle da thread atual.
        /// </summary>
        /// <remaks>
        /// Window.Start() precisa estar funcionando na thread principal para que a janela seja exibida corretamente.
        /// </remaks>
        public abstract void Run();
    }
}
