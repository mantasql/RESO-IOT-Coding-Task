using SensorApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorSimulator.Services
{
    public interface IDeviceService<T, R>
    {
        public Task<T> GetDeviceAsync(int deviceId);
        public Task PostTelemetryAsync(List<R> telemetryData, int deviceId);
        public Task<bool> IsApiReadyAsync();
    }
}
