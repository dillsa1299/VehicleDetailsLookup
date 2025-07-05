using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleDetailsLookup.Migrations
{
    /// <inheritdoc />
    public partial class AiDataHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DataHash",
                table: "AiData",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataHash",
                table: "AiData");
        }
    }
}
