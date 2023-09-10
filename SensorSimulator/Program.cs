using DomainModel.Models;
using DomainModel.Repo;
using SensorApi.ViewModels;
using SensorSimulator.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<SensorBackgroundService>();
        services.AddSingleton<IDeviceService<LightSensor, LightTelemetryInputViewModel>, LightSensorDeviceService>();
    })
    .Build();

await host.RunAsync();
