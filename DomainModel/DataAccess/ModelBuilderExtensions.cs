using DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.DataAccess
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LightSensor>().HasData(
                new LightSensor()
                {
                    Id = 1,
                    MeasurementFrequency = TimeSpan.FromMinutes(15),
                    SendingDataToServerFrequency = TimeSpan.FromHours(1)
                });
        }
    }
}
