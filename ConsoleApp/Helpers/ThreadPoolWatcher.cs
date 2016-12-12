using System;
using System.Collections.Generic;
using System.Threading;

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
            //I'm checking ThreadPool using by IO operations at database. So do not use same ThreadPool here for watching
            //Creating threads is expensive and do not use it at production code - only here for testing ThreadPool using by IO operations
            new Thread(() =>
            //Task.Run(() =>
            {
                _CheckingThreadId = Thread.CurrentThread.ManagedThreadId;
                while (!_StopWatching)
                {
                    var usedWorkerThreads = ActualUsedWorkerThreads();
                    _ThreadsAvailable.Add(new Tuple<DateTime, int>(DateTime.Now, usedWorkerThreads));
                    Thread.Sleep(checkingDelay);
                }
            }).Start();
        }

        public int ActualUsedWorkerThreads()
        {
            int availableCompletionPortThreads;
            int availableWorkerThreads;
            ThreadPool.GetAvailableThreads(out availableWorkerThreads, out availableCompletionPortThreads);
            int maxCompletionPortThreads;
            int maxWorkerThreads;
            ThreadPool.GetMaxThreads(out maxWorkerThreads, out maxCompletionPortThreads);
            return maxWorkerThreads-availableWorkerThreads;
        }


        public void EndWatchingAndLogWatchingResults()
        {
            _StopWatching = true;
            logger.Log($"ThreadPoolWatcher ThreadId was : {_CheckingThreadId} - not from ThreadPool");
            _ThreadsAvailable.ForEach(item => logger.LogWithColor($"ThreadPoolu using at time {item.Item1:HH:mm:ss.fff} WorkerThreads={item.Item2}", ConsoleColor.Green));
        }
    }
}