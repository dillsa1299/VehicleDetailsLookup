using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleDetailsLookup.Migrations
{
    /// <inheritdoc />
    public partial class defect_id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "MotDefects",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "MotDefects");
        }
    }
}
