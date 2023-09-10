using DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.DataAccess
{
    public class SensorContext : DbContext
    {
        public DbSet<Sensor> Sensors { get; set; } = null!;
        public DbSet<LightSensor> LightSensors { get; set; } = null!;
        public DbSet<Telemetry> Telemetries { get; set; } = null!;
        public DbSet<LightTelimetry> LightTelimetries { get; set; } = null!;

        public SensorContext(DbContextOptions<SensorContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sensor>().HasMany(c => c.Telemetries).WithOne(a => a.Sensor).HasForeignKey(a => a.SensorId);
            modelBuilder.Entity<Sensor>().HasKey(x => x.Id);
            modelBuilder.Entity<Telemetry>().HasOne(x => x.Sensor);
            modelBuilder.Entity<Telemetry>().HasKey(x => x.Id);

            modelBuilder.Seed();
        }
    }
}
