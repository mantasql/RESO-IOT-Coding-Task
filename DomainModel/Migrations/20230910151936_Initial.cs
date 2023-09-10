using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomainModel.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeasurementFrequency = table.Column<TimeSpan>(type: "time", nullable: false),
                    SendingDataToServerFrequency = table.Column<TimeSpan>(type: "time", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Telemetries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeasurementTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    SensorId = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Illuminance = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telemetries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Telemetries_Sensors_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Sensors",
                columns: new[] { "Id", "Discriminator", "MeasurementFrequency", "SendingDataToServerFrequency" },
                values: new object[] { 1, "LightSensor", new TimeSpan(0, 0, 15, 0, 0), new TimeSpan(0, 1, 0, 0, 0) });

            migrationBuilder.CreateIndex(
                name: "IX_Telemetries_SensorId",
                table: "Telemetries",
                column: "SensorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Telemetries");

            migrationBuilder.DropTable(
                name: "Sensors");
        }
    }
}
