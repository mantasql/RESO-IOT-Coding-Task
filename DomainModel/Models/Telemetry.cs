using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DomainModel.Models
{
    public class Telemetry
    {
        public int Id { get; set; }

        [Required]
        public DateTimeOffset MeasurementTime { get; set; }

        [ForeignKey(nameof(SensorId))]
        [JsonIgnore]
        public virtual Sensor? Sensor { get; set; }
        public int SensorId { get; set; }
    }
}
