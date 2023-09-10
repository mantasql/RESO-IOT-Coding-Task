using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models
{
    public class LightSensor : Sensor
    {
        public override Units Units => Units.Lux;
    }
}
