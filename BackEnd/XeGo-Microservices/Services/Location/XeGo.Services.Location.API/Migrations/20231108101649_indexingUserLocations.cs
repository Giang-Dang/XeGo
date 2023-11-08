using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XeGo.Services.Location.API.Migrations
{
    /// <inheritdoc />
    public partial class indexingUserLocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserLocations",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Geohash",
                table: "UserLocations",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_UserLocations_Geohash",
                table: "UserLocations",
                column: "Geohash");

            migrationBuilder.CreateIndex(
                name: "IX_UserLocations_UserId",
                table: "UserLocations",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserLocations_Geohash",
                table: "UserLocations");

            migrationBuilder.DropIndex(
                name: "IX_UserLocations_UserId",
                table: "UserLocations");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserLocations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Geohash",
                table: "UserLocations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
