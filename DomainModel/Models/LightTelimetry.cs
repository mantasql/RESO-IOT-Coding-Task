using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models
{
    public class LightTelimetry : Telemetry
    {
        public float Illuminance { get; set; }
    }
}
