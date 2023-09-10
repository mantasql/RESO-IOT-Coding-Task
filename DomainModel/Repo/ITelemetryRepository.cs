using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Repo
{
    public interface ITelemetryRepository
    {
        public Task SaveTelemetryDataAsync<T>(IEnumerable<T> telemetries) where T : Telemetry;
    }
}
