using Soaryoto.Interfaces;
using Soaryoto.Utils;
using Gma.System.MouseKeyHook;

namespace Soaryoto
{
    public class ButtonInterceptor : IButtonInterceptor
    {
        private IKeyboardMouseEvents? _globalHook;

        public void StartService()
        {
            Start();
        }
        public void StopService()
        {
            Stop();
        }


        private void Start()
        {
            _globalHook = Hook.GlobalEvents();
            _globalHook.KeyDown += GlobalHook_KeyDown;

            ConsoleWriter.WriteLine("[BTN] Глобальный перехват запущен.", ConsoleColor.Green);
            ConsoleWriter.WriteLine("[BTN] Нажмите Esc в окне консоли, чтобы завершить.", ConsoleColor.Green);
        }

        private void Stop()
        {
            if (_globalHook != null)
            {
                _globalHook.KeyDown -= GlobalHook_KeyDown;
                _globalHook.Dispose();
                _globalHook = null;
                ConsoleWriter.WriteLine("[BTN] Захват остановлен.", ConsoleColor.Red);
            }
        }

        private void GlobalHook_KeyDown(object? sender, KeyEventArgs e)
        {
            ConsoleWriter.WriteLine($"[BTN] KeyDown: {e.KeyCode}", ConsoleColor.White);
            
            if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }
        }
    }
}