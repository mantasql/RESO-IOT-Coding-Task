using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Models
{
    public abstract class Sensor
    {
        public int Id { get; set; }
        public abstract Units Units { get; }

        [Required]
        public virtual TimeSpan MeasurementFrequency { get; set; }
        public virtual TimeSpan SendingDataToServerFrequency { get; set; }

        public virtual List<Telemetry> Telemetries { get; set; } = null!;
    }

    public enum Units
    {
        Lux,
        Celcius
    }
}