using DomainModel.DataAccess;
using DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Repo
{
    public class EFSensorRepository : ISensorRepository
    {
        private readonly SensorContext context;

        public EFSensorRepository(SensorContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IQueryable<Sensor> Sensors => context.Sensors.Include(s => s.Telemetries);

        public async Task AddSensorAsync(Sensor sensor)
        {
            if (sensor is null)
            {
                throw new ArgumentNullException(nameof(sensor));
            }

            await context.Sensors.AddAsync(sensor);
            await context.SaveChangesAsync();
        }

        public async Task<TSensor> GetSensorByIdAsync<TSensor>(int id) where TSensor : Sensor
        {
            TSensor? sensor = await context.Sensors.Include(l => l.Telemetries).OfType<TSensor>().FirstOrDefaultAsync(x => x.Id == id);

            if (sensor is null)
            {
                throw new ArgumentException($"No sensor with id: {id}");
            }

            return sensor;

        }
    }
}
