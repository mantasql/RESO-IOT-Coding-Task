using DomainModel.Models;
using DomainModel.Repo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SensorApi.Controllers
{
    [Route("devices")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly ISensorRepository sensorRepository;

        public SensorController(ISensorRepository sensorRepository)
        {
            this.sensorRepository = sensorRepository;
        }

        [HttpGet("{deviceId}")]
        public async Task<IActionResult> GetSensorById(int deviceId)
        {
            try
            {
                Sensor sensor = await sensorRepository.GetSensorByIdAsync<Sensor>(deviceId);
                return Ok(sensor);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
