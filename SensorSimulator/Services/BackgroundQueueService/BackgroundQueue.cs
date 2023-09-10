using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorSimulator.Services.BackgroundQueueService
{
    public class BackgroundQueue<T> : IBackgroundQueue<T> where T : class
    {
        private readonly ConcurrentQueue<T> items = new ConcurrentQueue<T>();

        public void Enqueue(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            items.Enqueue(item);
        }

        public T? Dequeue()
        {
            var success = items.TryDequeue(out var item);

            return success ? item : null;
        }

        public T? GetItem()
        {
            items.TryPeek(out var item);

            return item;
        }
    }
}
