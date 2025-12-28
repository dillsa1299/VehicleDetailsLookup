using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleDetailsLookup.Migrations
{
    /// <inheritdoc />
    public partial class AI_Data_MetaData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MetaData",
                table: "AiData",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MetaData",
                table: "AiData");
        }
    }
}
