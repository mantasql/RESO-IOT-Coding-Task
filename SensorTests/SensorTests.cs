using DomainModel.DataAccess;
using DomainModel.Models;
using DomainModel.Repo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace SensorTests
{
    [TestFixture]
    public class SensorTests
    {
        private readonly DbContextOptions<SensorContext> dbContextOptions;
        private ISensorRepository sensorRepository;
        private ITelemetryRepository telemetryRepository;


        public SensorTests()
        {
            var dbName = $"SensorDb_{DateTime.Now.ToFileTimeUtc()}";
            dbContextOptions = new DbContextOptionsBuilder<SensorContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
        }

        [SetUp]
        public async Task Init()
        {
            this.sensorRepository = await CreateSensorRepositoryAsync();
            this.telemetryRepository = CreateTelemetryRepository();
        }

        [Test]
        public async Task SensorRepository_AddSensor_Valid()
        {
            LightSensor actual = new LightSensor()
            {
                MeasurementFrequency = new TimeSpan(0, 0, 30),
                SendingDataToServerFrequency = new TimeSpan(0, 1, 0),
                Telemetries = new List<Telemetry>()
                {
                    new LightTelimetry()
                    {
                        Illuminance = 100.5f,
                        MeasurementTime = new DateTime(2023, 6, 22)
                    }
                }
            };

            await sensorRepository.AddSensorAsync(actual);
            Sensor expected = await sensorRepository.GetSensorByIdAsync<LightSensor>(actual.Id);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SensorRepository_AddSensor_NullSensor_ThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await sensorRepository.AddSensorAsync(null));
        }

        [Test]
        public void SensorRepository_GetSensorById_NonExising_ThrowArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await sensorRepository.GetSensorByIdAsync<Sensor>(0));
        }

        [Test]
        public async Task TelemetryRepository_SaveTelemetryData_IsValid()
        {
            LightSensor sensor = await sensorRepository.GetSensorByIdAsync<LightSensor>(1);

            List<Telemetry> telimetryData = new List<Telemetry>()
            {
                new LightTelimetry()
                {
                    Illuminance = 200,
                    MeasurementTime = DateTimeOffset.UtcNow,
                    SensorId = sensor.Id,
                },
                new Telemetry()
                {
                    SensorId = sensor.Id,
                    MeasurementTime = DateTimeOffset.UtcNow,
                }
            };

            int expectedTelemetryCount = sensor.Telemetries.Count + telimetryData.Count;

            await telemetryRepository.SaveTelemetryDataAsync(telimetryData);
            sensor = await sensorRepository.GetSensorByIdAsync<LightSensor>(1);

            int actualTelemetryCount = sensor.Telemetries.Count;

            Assert.AreEqual(expectedTelemetryCount, actualTelemetryCount);
        }

        [Test]
        public void SensorRepository_SaveTelemetryData_NullData_ThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await telemetryRepository.SaveTelemetryDataAsync<Telemetry>(null));
        }

        private async Task<ISensorRepository> CreateSensorRepositoryAsync()
        {
            SensorContext context = new SensorContext(dbContextOptions);

            await PopulateDataAsync(context);

            return new EFSensorRepository(context);
        }

        private ITelemetryRepository CreateTelemetryRepository()
        {
            SensorContext context = new SensorContext(dbContextOptions);

            return new EFTelemetryRepository(context);
        }

        private async Task PopulateDataAsync(SensorContext context)
        {
            LightSensor sensor = new LightSensor()
            {
                MeasurementFrequency = new TimeSpan(0,0,20),
                SendingDataToServerFrequency = new TimeSpan(0,1,0),
                Telemetries = new List<Telemetry>
                {
                    new LightTelimetry()
                    {
                        Illuminance = 123.5f,
                        MeasurementTime = DateTimeOffset.UtcNow,
                    },
                    new LightTelimetry()
                    {
                        Illuminance = 123.0f,
                        MeasurementTime = new DateTime(2023, 8, 10),
                    },
                    new LightTelimetry()
                    {
                        Illuminance = 122.5f,
                        MeasurementTime = new DateTime(2023, 7, 10),
                    }
                }
            };

            await context.Sensors.AddAsync(sensor);
            await context.SaveChangesAsync();
        }
    }
}