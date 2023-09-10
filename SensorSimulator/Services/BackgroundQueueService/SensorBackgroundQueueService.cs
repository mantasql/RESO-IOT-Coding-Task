using DomainModel.Models;
using DomainModel.Repo;
using SensorApi.ViewModels;
using SensorSimulator.SensorData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SensorSimulator.Services.BackgroundQueueService
{
    public class SensorBackgroundQueueService : BackgroundService
    {
        private readonly IBackgroundQueue<LightSensor> queue;
        private readonly IDeviceService<LightSensor, LightTelemetryInputViewModel> deviceService;
        private readonly ILogger<SensorBackgroundQueueService> logger;

        public SensorBackgroundQueueService(IBackgroundQueue<LightSensor> queue, IDeviceService<LightSensor, LightTelemetryInputViewModel> deviceService, ILogger<SensorBackgroundQueueService> logger)
        {
            this.queue = queue;
            this.deviceService = deviceService;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
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
                    await Task.Delay(1000, stoppingToken);

                    var sensor = queue.GetItem();

                    if (sensor == null)
                    {
                        continue;
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
