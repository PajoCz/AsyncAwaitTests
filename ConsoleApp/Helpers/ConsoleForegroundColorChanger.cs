using System;

namespace ConsoleApp.Helpers
{
    public class ConsoleForegroundColorChanger : IDisposable
    {
        private readonly ConsoleColor _OriginalColor;

        public ConsoleForegroundColorChanger(ConsoleColor p_NewColor)
        {
            _OriginalColor = Console.ForegroundColor;
            Console.ForegroundColor = p_NewColor;
        }

        public void Dispose()
        {
            Console.ForegroundColor = _OriginalColor;
        }
    }
}