using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace XeGo.Services.Price.API.Migrations
{
    /// <inheritdoc />
    public partial class seedData1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "VehicleTypePrices",
                columns: new[] { "VehicleTypeId", "CreatedBy", "CreatedDate", "DropCharge", "LastModifiedBy", "LastModifiedDate", "PricePerKm" },
                values: new object[,]
                {
                    { 1, "SYSTEM", new DateTime(2023, 12, 1, 10, 49, 30, 184, DateTimeKind.Utc).AddTicks(9660), 1.0, "SYSTEM", new DateTime(2023, 12, 1, 10, 49, 30, 184, DateTimeKind.Utc).AddTicks(9661), 1.0 },
                    { 2, "SYSTEM", new DateTime(2023, 12, 1, 10, 49, 30, 184, DateTimeKind.Utc).AddTicks(9664), 1.5, "SYSTEM", new DateTime(2023, 12, 1, 10, 49, 30, 184, DateTimeKind.Utc).AddTicks(9664), 1.5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VehicleTypePrices",
                keyColumn: "VehicleTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "VehicleTypePrices",
                keyColumn: "VehicleTypeId",
                keyValue: 2);
        }
    }
}
