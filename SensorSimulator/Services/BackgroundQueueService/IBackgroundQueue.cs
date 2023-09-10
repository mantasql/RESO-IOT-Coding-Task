using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorSimulator.Services.BackgroundQueueService
{
    public interface IBackgroundQueue<T>
    {
        void Enqueue(T item);
        T? Dequeue();
        T? GetItem();
    }
}
