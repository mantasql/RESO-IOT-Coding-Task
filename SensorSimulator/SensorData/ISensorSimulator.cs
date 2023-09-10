using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorSimulator.SensorData
{
    public interface ISensorSimulator<T> where T : Telemetry
    {
        public T SimulateData(Sensor sensor);
    }
}
