using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleDetailsLookup.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AiData",
                columns: table => new
                {
                    RegistrationNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    GeneratedText = table.Column<string>(type: "TEXT", nullable: true),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiData", x => new { x.RegistrationNumber, x.Type });
                });

            migrationBuilder.CreateTable(
                name: "Details",
                columns: table => new
                {
                    RegistrationNumber = table.Column<string>(type: "TEXT", nullable: false),
                    YearOfManufacture = table.Column<int>(type: "INTEGER", nullable: false),
                    Make = table.Column<string>(type: "TEXT", nullable: true),
                    Model = table.Column<string>(type: "TEXT", nullable: true),
                    Colour = table.Column<string>(type: "TEXT", nullable: true),
                    EngineCapacity = table.Column<string>(type: "TEXT", nullable: true),
                    FuelType = table.Column<string>(type: "TEXT", nullable: true),
                    TaxStatus = table.Column<string>(type: "TEXT", nullable: true),
                    TaxDueDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    MotStatus = table.Column<string>(type: "TEXT", nullable: true),
                    MotExpiryDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    DateOfLastV5CIssued = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    MonthOfFirstRegistration = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Details", x => x.RegistrationNumber);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    RegistrationNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => new { x.RegistrationNumber, x.Url });
                });

            migrationBuilder.CreateTable(
                name: "MotDefects",
                columns: table => new
                {
                    TestNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Dangerous = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotDefects", x => new { x.TestNumber, x.Description });
                });

            migrationBuilder.CreateTable(
                name: "MotTests",
                columns: table => new
                {
                    TestNumber = table.Column<string>(type: "TEXT", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "TEXT", nullable: true),
                    CompletedDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Passed = table.Column<bool>(type: "INTEGER", nullable: false),
                    ExpiryDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    OdometerValue = table.Column<long>(type: "INTEGER", nullable: false),
                    OdometerUnit = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotTests", x => x.TestNumber);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AiData");

            migrationBuilder.DropTable(
                name: "Details");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "MotDefects");

            migrationBuilder.DropTable(
                name: "MotTests");
        }
    }
}
