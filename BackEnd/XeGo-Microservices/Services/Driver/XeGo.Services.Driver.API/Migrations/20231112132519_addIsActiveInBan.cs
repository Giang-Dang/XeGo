using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XeGo.Services.Driver.API.Migrations
{
    /// <inheritdoc />
    public partial class addIsActiveInBan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "VehicleBans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "DriverBans",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "VehicleBans");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "DriverBans");
        }
    }
}
