using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XeGo.Services.Location.API.Migrations
{
    /// <inheritdoc />
    public partial class addIsDriver : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDriver",
                table: "UserLocations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDriver",
                table: "UserLocations");
        }
    }
}
