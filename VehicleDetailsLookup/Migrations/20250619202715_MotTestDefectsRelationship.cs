using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleDetailsLookup.Migrations
{
    /// <inheritdoc />
    public partial class MotTestDefectsRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_MotDefects_MotTests_TestNumber",
                table: "MotDefects",
                column: "TestNumber",
                principalTable: "MotTests",
                principalColumn: "TestNumber",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MotDefects_MotTests_TestNumber",
                table: "MotDefects");
        }
    }
}
