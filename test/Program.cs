using RTIO;
using System;
using System.Collections;
using System.Windows.Forms;

namespace RTIO_Test
{
    static class Program
    {
        static System.Action a;

        [STAThread]
        static void Main()
        {
            Image image = new(200, 300);
            image.Foreach((_) => new (240, 240, 240));
            Window window = new(image);

            window.OnKeyDown += (key) => { Console.WriteLine($"{key} is down."); };
            window.OnKeyUp += (key) => { Console.WriteLine($"{key} is up."); };
            window.OnFocus += () => { Console.WriteLine($"Window focused."); };
            window.OnFocusOut += () => { Console.WriteLine($"Window unfocused."); };
            window.OnMouseDown += (pos) => { Console.WriteLine($"Mouse down at {pos}."); };
            window.OnMouseUp += (pos) => { Console.WriteLine($"Mouse up at {pos}."); };
            window.OnUpdate += (_) => { image.Foreach((color) => new((byte)((color.R + 1) % 200), (byte)((color.G + 2) % 180), (byte)((color.B + 4) % 256))); };

            window.Run();
        }
    }
}