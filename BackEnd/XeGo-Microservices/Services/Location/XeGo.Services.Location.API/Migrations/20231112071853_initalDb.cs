using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XeGo.Services.Location.API.Migrations
{
    /// <inheritdoc />
    public partial class initalDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DriverLocations",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Geohash = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverLocations", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "RiderLocations",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Geohash = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiderLocations", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DriverLocations_Geohash",
                table: "DriverLocations",
                column: "Geohash");

            migrationBuilder.CreateIndex(
                name: "IX_DriverLocations_UserId",
                table: "DriverLocations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RiderLocations_Geohash",
                table: "RiderLocations",
                column: "Geohash");

            migrationBuilder.CreateIndex(
                name: "IX_RiderLocations_UserId",
                table: "RiderLocations",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverLocations");

            migrationBuilder.DropTable(
                name: "RiderLocations");
        }
    }
}
