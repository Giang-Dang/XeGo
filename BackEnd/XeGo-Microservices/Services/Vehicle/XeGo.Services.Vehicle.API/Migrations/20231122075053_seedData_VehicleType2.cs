using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XeGo.Services.Vehicle.API.Migrations
{
    /// <inheritdoc />
    public partial class seedData_VehicleType2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "VehicleTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2023, 11, 22, 7, 50, 52, 770, DateTimeKind.Utc).AddTicks(7857), new DateTime(2023, 11, 22, 7, 50, 52, 770, DateTimeKind.Utc).AddTicks(7857) });

            migrationBuilder.UpdateData(
                table: "VehicleTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2023, 11, 22, 7, 50, 52, 770, DateTimeKind.Utc).AddTicks(7860), new DateTime(2023, 11, 22, 7, 50, 52, 770, DateTimeKind.Utc).AddTicks(7861) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "VehicleTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2023, 11, 22, 7, 49, 19, 193, DateTimeKind.Utc).AddTicks(6749), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "VehicleTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2023, 11, 22, 7, 49, 19, 193, DateTimeKind.Utc).AddTicks(6753), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
