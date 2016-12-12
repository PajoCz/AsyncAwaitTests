using System;
using System.Diagnostics;

namespace ConsoleApp.Helpers
{
    public abstract class TestSqlBase
    {
        protected readonly Logger Logger;
        protected readonly SqlCaller SqlCaller;

        protected TestSqlBase()
        {
            Logger = new Logger();
            SqlCaller = new SqlCaller(Logger);
        }

        public void Run(string p_Description, int p_CallCount, TimeSpan p_SqlDelay)
        {
            var threadPoolWatcher = new ThreadPoolWatcher();
            threadPoolWatcher.StartWatching();
            Logger.LogWithColor($"START {p_Description} ({p_CallCount}x {p_SqlDelay})", ConsoleColor.Red);
            var sw = Stopwatch.StartNew();
            var sum = RunImpl(p_CallCount, p_SqlDelay);
            sw.Stop();
            var calculator = new Calculator();
            var calculated = calculator.SumNumbersFromOneTo(p_CallCount);
            Logger.LogWithColor($"END {p_Description} sum response values={sum} (must be {calculated}) elapsed {sw.Elapsed}", ConsoleColor.Red);
            threadPoolWatcher.EndWatchingAndLogWatchingResults();
        }

        protected abstract int RunImpl(int callCount, TimeSpan sqlDelay);
    }
}