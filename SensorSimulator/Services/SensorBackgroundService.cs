using DomainModel.Models;
using DomainModel.Repo;
using Newtonsoft.Json;
using SensorApi.ViewModels;
using SensorSimulator.SensorData;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace SensorSimulator.Services
{
    public class SensorBackgroundService : BackgroundService
    {
        private readonly ILogger<SensorBackgroundService> logger;
        private readonly IDeviceService<LightSensor, LightTelemetryInputViewModel> deviceService;
        private readonly int delayApiCheckMillis = 5000;
        private readonly int deviceId = 1;
        private LightSensor? sensor;

        public SensorBackgroundService(ILogger<SensorBackgroundService> logger, IDeviceService<LightSensor, LightTelemetryInputViewModel> deviceService)
        {
            this.logger = logger;
            this.deviceService = deviceService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!await deviceService.IsApiReadyAsync())
            {
                await Task.Delay(delayApiCheckMillis, stoppingToken);
            }

            await SensorProcessing(stoppingToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        private async Task SensorProcessing(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (sensor is null)
                    {
                        sensor = await deviceService.GetDeviceAsync(deviceId);
                    }

                    var telemetryData = new List<LightTelemetryInputViewModel>();
                    var startTime = DateTimeOffset.UtcNow;

                    while (DateTimeOffset.UtcNow - startTime <= sensor.SendingDataToServerFrequency)
                    {
                        LightTelimetry telimetry = new LightSensorSimulator().SimulateData(sensor);

                        var telemetryViewModel = new LightTelemetryInputViewModel
                        {
                            Illum = telimetry.Illuminance,
                            Time = telimetry.MeasurementTime.ToUnixTimeSeconds()
                        };

                        telemetryData.Add(telemetryViewModel);

                        Debug.WriteLine(DateTimeOffset.UtcNow - startTime);

                        await Task.Delay(sensor.MeasurementFrequency, stoppingToken);
                    }

                    await deviceService.PostTelemetryAsync(telemetryData, sensor.Id);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred during device simulation.");
                }
            }
        }
    }
}