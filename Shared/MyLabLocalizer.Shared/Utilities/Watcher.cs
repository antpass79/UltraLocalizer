using System;
using System.Diagnostics;

namespace MyLabLocalizer.Shared.Utilities
{
    public class Watcher : IDisposable
    {
        private readonly string _message;

        DateTime _start;
        Stopwatch _stopwatch;

        public Watcher(string message)
        {
            _message = message;
            Start();
        }

        private void Start()
        {
            _start = DateTime.Now;
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _stopwatch.Stop();

                    Trace.WriteLine(string.Empty);
                    Trace.WriteLine($"{_message}: Activity starts at {_start} and stops at {DateTime.Now} with duration of {_stopwatch.Elapsed}");
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}