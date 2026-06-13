using System;
using System.Threading;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;

namespace Soaryoto {
    internal class Program
    {
        private static IKeyboardMouseEvents? _globalHook;
        static void Main(string[] args)
        {
            _globalHook = Hook.GlobalEvents();
            _globalHook.KeyDown += GlobalHook_KeyDown;

            Console.WriteLine("Глобальный перехват запущен.");
            Console.WriteLine("Нажмите Esc в окне консоли, чтобы завершить.");

            Application.Run();

            _globalHook.KeyDown -= GlobalHook_KeyDown;
            _globalHook.Dispose();
        }

        private static void GlobalHook_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine($"KeyDown: {e.KeyCode}");
            
            if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }
        }

    }
}