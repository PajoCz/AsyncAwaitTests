using System;
using System.Threading;

namespace ConsoleApp.Helpers
{
    public class Logger
    {
        public void Log(string p_Text)
        {
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.fff")} : Thread{Thread.CurrentThread.ManagedThreadId:00} : {p_Text}");
        }

        public void LogWithColor(string p_Text, ConsoleColor p_ConsoleColor)
        {
            using (new ConsoleForegroundColorChanger(p_ConsoleColor))
            {
                Log(p_Text);
            }
        }
    }
}