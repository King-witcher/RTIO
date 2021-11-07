using System;

namespace RTIO
{
    public interface IWindow
    {
        event Action<TimeSpan> OnUpdate;
        event Action<Key> OnKeyDown;
        event Action<Key> OnKeyUp;
        event Action<(int x, int y)> OnMouseDown;
        event Action<(int x, int y)> OnMouseUp;
        event Action OnFocus;
        event Action OnFocusOut;

        (int width, int height) Dimensions { get; set; }
        bool Maximized { get; set; }
    }
}
