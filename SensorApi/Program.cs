using DomainModel.DataAccess;
using DomainModel.Models;
using DomainModel.Repo;
using Microsoft.EntityFrameworkCore;

namespace SensorApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHealthChecks();
            builder.Services.AddDbContext<SensorContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("SensorConnection"));
            });
            builder.Services.AddScoped<ITelemetryRepository, EFTelemetryRepository>();
            builder.Services.AddScoped<ISensorRepository, EFSensorRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapHealthChecks("/health-check");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}