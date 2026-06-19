namespace Soaryoto.Utils
{
    public class ConsoleWriter
    {
        private static readonly object _lock = new();

        public static void WriteLine(string message, ConsoleColor color = ConsoleColor.White)
        {
            lock (_lock)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }
    }
}