using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorSimulator.SensorData
{
    public class LightSensorSimulator : ISensorSimulator<LightTelimetry>
    {
        public LightTelimetry SimulateData(Sensor sensor)
        {
            if (sensor is null)
            {
                throw new ArgumentNullException(nameof(sensor));
            }

            DateTimeOffset currentTime = DateTimeOffset.UtcNow;

            float illuminance = GetIlluminanceValue(currentTime);

            LightTelimetry telimetry = new LightTelimetry
            {
                Illuminance = illuminance,
                MeasurementTime = currentTime
            };

            return telimetry;
        }

        private float GetIlluminanceValue(DateTimeOffset currentTime)
        {
            float minIlluminance = 1;
            float maxIlluminance = 20000;
            double minutesPassed = currentTime.TimeOfDay.TotalMinutes;

            if (minutesPassed < 720)
            {
                return (float)Math.Round((minIlluminance + (maxIlluminance - minIlluminance) * minutesPassed / 720) * 2) / 2;
            }
            else
            {
                return (float)Math.Round((maxIlluminance - (maxIlluminance - minIlluminance) * ((minutesPassed - 720) / 720)) * 2) / 2;
            }
        }
    }
}
