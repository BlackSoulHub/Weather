using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Weather.Web.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Records",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Temperature = table.Column<double>(type: "REAL", nullable: false),
                    Humidity = table.Column<double>(type: "REAL", nullable: false),
                    Td = table.Column<double>(type: "REAL", nullable: false),
                    AirPressure = table.Column<double>(type: "REAL", nullable: false),
                    WindDirection = table.Column<string>(type: "TEXT", nullable: true),
                    WindSpeed = table.Column<int>(type: "INTEGER", nullable: true),
                    Cloudiness = table.Column<byte>(type: "INTEGER", nullable: true),
                    LowCloudiness = table.Column<int>(type: "INTEGER", nullable: true),
                    VV = table.Column<double>(type: "REAL", nullable: true),
                    WeatherEvent = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Records", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Records");
        }
    }
}
