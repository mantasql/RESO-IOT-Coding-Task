using DomainModel.DataAccess;
using DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Repo
{
    public class EFTelemetryRepository : ITelemetryRepository
    {
        private readonly SensorContext context;

        public EFTelemetryRepository(SensorContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task SaveTelemetryDataAsync<T>(IEnumerable<T> telimetryData) where T : Telemetry
        {
            if (telimetryData is null)
            {
                throw new ArgumentNullException(nameof(telimetryData));
            }

            await context.Telemetries.AddRangeAsync(telimetryData);
            await context.SaveChangesAsync();
        }
    }
}
