using DomainModel.Models;
using Newtonsoft.Json;
using SensorApi.ViewModels;
using SensorSimulator.SensorData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SensorSimulator.Services
{
    public class LightSensorDeviceService : IDeviceService<LightSensor, LightTelemetryInputViewModel>
    {
        private readonly HttpClient httpClient;
        public LightSensorDeviceService()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:7249");
        }

        public async Task<LightSensor> GetDeviceAsync(int deviceId)
        {
            var response = await httpClient.GetAsync($"/devices/{deviceId}");

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                LightSensor? device = JsonConvert.DeserializeObject<LightSensor>(responseBody);

                if (device is null)
                {
                    throw new JsonException($"Device with desired id couldn't be deserialized");
                }

                return device;
            }

            throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
        }

        public async Task PostTelemetryAsync(List<LightTelemetryInputViewModel> telemetryData, int deviceId)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"/devices/{deviceId}/telemetry", telemetryData);
                if (response.IsSuccessStatusCode)
                {
                    await Console.Out.WriteLineAsync("Posted telemetry data");
                }

            } catch (HttpRequestException ex)
            {
                await Console.Out.WriteLineAsync("Error posting telemetry data: " + ex.Message);
            }

        }

        public async Task<bool> IsApiReadyAsync()
        {
            try
            {
                var responce = await httpClient.GetAsync($"/health-check");
                return responce.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Failed to establish connection to API: " + ex.Message);
                return false;
            }
        }
    }
}
