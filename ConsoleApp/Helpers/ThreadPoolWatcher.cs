using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp.Helpers
{
    public class ThreadPoolWatcher
    {
        private readonly TimeSpan _CheckingDelayDefault = TimeSpan.FromMilliseconds(500);
        private readonly Logger logger = new Logger();
        private bool _StopWatching;
        private int _CheckingThreadId;
        private List<Tuple<DateTime, int>> _ThreadsAvailable = new List<Tuple<DateTime, int>>();

        public void StartWatching(TimeSpan? p_OverrideCheckingDelay = null)
        {
            var checkingDelay = p_OverrideCheckingDelay ?? _CheckingDelayDefault;
            _ThreadsAvailable = new List<Tuple<DateTime, int>>();
            _StopWatching = false;
            Task.Run(() =>
            {
                _CheckingThreadId = Thread.CurrentThread.ManagedThreadId;
                while (!_StopWatching)
                {
                    int availableCompletionPortThreads;
                    int availableWorkerThreads;
                    ThreadPool.GetAvailableThreads(out availableWorkerThreads, out availableCompletionPortThreads);
                    int maxCompletionPortThreads;
                    int maxWorkerThreads;
                    ThreadPool.GetMaxThreads(out maxWorkerThreads, out maxCompletionPortThreads);
                    _ThreadsAvailable.Add(new Tuple<DateTime, int>(DateTime.Now, maxWorkerThreads - availableWorkerThreads));
                    Thread.Sleep(checkingDelay);
                }
            });
        }

        public void EndWatchingAndLogWatchingResults()
        {
            _StopWatching = true;
            logger.Log($"ThreadPoolWatcher ThreadId was : {_CheckingThreadId}");
            _ThreadsAvailable.ForEach(item => logger.LogWithColor($"ThreadPoolu using at time {item.Item1:HH:mm:ss.fff} WorkerThreads={item.Item2}", ConsoleColor.Green));
        }
    }
}