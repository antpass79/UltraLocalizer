using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLabLocalizer.Core.Utilities
{
    public class ParallelEnumerableResultTaskRunner<TItemResult> : IDisposable
    {
        readonly List<Task<IEnumerable<TItemResult>>> _tasks = new List<Task<IEnumerable<TItemResult>>>();
        private bool disposedValue;

        public void Add(Task<IEnumerable<TItemResult>> task)
        {
            _tasks.Add(task);
        }
        public void AddRange(IEnumerable<Task<IEnumerable<TItemResult>>> tasks)
        {
            _tasks.AddRange(tasks);
        }
        public async Task<IEnumerable<TItemResult>> RunAsync()
        {
            var results = await Task.WhenAll(_tasks);
            return await Task.FromResult(results.SelectMany(result => result).ToList());
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _tasks.Clear();
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
