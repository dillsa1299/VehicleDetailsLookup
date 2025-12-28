using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleDetailsLookup.Migrations
{
    /// <inheritdoc />
    public partial class AI_Data_MetaData_Index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AiData",
                table: "AiData");

            migrationBuilder.AlterColumn<string>(
                name: "MetaData",
                table: "AiData",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AiData",
                table: "AiData",
                columns: new[] { "RegistrationNumber", "Type", "MetaData" });

            migrationBuilder.CreateIndex(
                name: "IX_AiData_RegistrationNumber_Type_MetaData",
                table: "AiData",
                columns: new[] { "RegistrationNumber", "Type", "MetaData" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AiData",
                table: "AiData");

            migrationBuilder.DropIndex(
                name: "IX_AiData_RegistrationNumber_Type_MetaData",
                table: "AiData");

            migrationBuilder.AlterColumn<string>(
                name: "MetaData",
                table: "AiData",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AiData",
                table: "AiData",
                columns: new[] { "RegistrationNumber", "Type" });
        }
    }
}
