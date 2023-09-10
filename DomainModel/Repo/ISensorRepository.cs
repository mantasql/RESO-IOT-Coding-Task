using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Repo
{
    public interface ISensorRepository
    {
        public IQueryable<Sensor> Sensors { get; }
        public Task AddSensorAsync(Sensor sensor);
        public Task<TSensor> GetSensorByIdAsync<TSensor>(int id) where TSensor : Sensor;
    }
}
