using DomainModel.Models;
using DomainModel.Repo;
using Microsoft.AspNetCore.Mvc;
using SensorApi.ViewModels;
using System.Collections;
using System.Diagnostics;

namespace SensorApi.Controllers
{
    [Route("devices")]
    [ApiController]
    public class TelemetryController : ControllerBase
    {
        private readonly ITelemetryRepository telemetryRepository;
        private readonly ISensorRepository sensorRepository;

        public TelemetryController(ITelemetryRepository telemetryRepository, ISensorRepository sensorRepository)
        {
            this.telemetryRepository = telemetryRepository ?? throw new ArgumentNullException(nameof(telemetryRepository));
            this.sensorRepository = sensorRepository ?? throw new ArgumentNullException(nameof(sensorRepository));
        }

        [HttpPost("{deviceId}/telemetry")]
        public async Task<IActionResult> PostLightTelemetry([FromBody] IEnumerable<LightTelemetryInputViewModel> telemetries, [FromRoute] int deviceId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var telemetryList = new List<LightTelimetry>();
            foreach (var telemetry in telemetries)
            {
                var statistics = new LightTelimetry
                {
                    Illuminance = telemetry.Illum,
                    MeasurementTime = DateTimeOffset.FromUnixTimeSeconds(telemetry.Time),
                    SensorId = deviceId
                };
                telemetryList.Add(statistics);
            }

            try
            {
                await telemetryRepository.SaveTelemetryDataAsync(telemetryList);
                return Ok(telemetryList);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error posting data into the database");
            }
        }

        [HttpGet("{deviceId}/statistics")]
        public async Task<IActionResult> GetLightStatistics([FromRoute] int deviceId)
        {
            DateTime thirtyDaysAgo = DateTime.UtcNow.Date.AddDays(-30);

            try
            {
                var sensor = await sensorRepository.GetSensorByIdAsync<LightSensor>(deviceId);

                var groupedData = sensor.Telemetries
                    .OfType<LightTelimetry>()
                    .Where(x => x.MeasurementTime >= thirtyDaysAgo)
                    .GroupBy(x => x.MeasurementTime.Date);

                var statistics = groupedData.Select(group =>
                    new LightTelemetryOutputViewModel
                    {
                        Date = group.Key,
                        MaxIlluminance = group.Max(x => x.Illuminance)
                    }).ToArray();

                return Ok(statistics);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
