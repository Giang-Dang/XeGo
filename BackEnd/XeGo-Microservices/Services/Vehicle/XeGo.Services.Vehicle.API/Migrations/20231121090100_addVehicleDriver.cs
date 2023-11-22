using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XeGo.Services.Vehicle.API.Migrations
{
    /// <inheritdoc />
    public partial class addVehicleDriver : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "VehicleDrivers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "VehicleDrivers");
        }
    }
}
