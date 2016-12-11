using System;
using ConsoleApp.Helpers;

namespace ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            new TestSqlSync().Run("1 : SYNC", 5, TimeSpan.FromSeconds(3));
            new TestSqlSyncParallel().Run("2 : SYNC Parallel", 20, TimeSpan.FromSeconds(10));
            new TestSqlAsync().Run("3 : ASYNC", 20, TimeSpan.FromSeconds(10));

            new Logger().LogWithColor("--- END ---", ConsoleColor.Yellow);

            Console.ReadLine();
        }
    }
}