using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XeGo.Services.Ride.API.Migrations
{
    /// <inheritdoc />
    public partial class _271123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VehicleId",
                table: "Rides",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "CodeValues",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedDate", "EffectiveStartDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2023, 11, 27, 11, 5, 28, 785, DateTimeKind.Utc).AddTicks(6091), new DateTime(2023, 11, 27, 11, 5, 28, 785, DateTimeKind.Utc).AddTicks(6088), new DateTime(2023, 11, 27, 11, 5, 28, 785, DateTimeKind.Utc).AddTicks(6092) });

            migrationBuilder.UpdateData(
                table: "CodeValues",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedDate", "EffectiveStartDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2023, 11, 27, 11, 5, 28, 785, DateTimeKind.Utc).AddTicks(6096), new DateTime(2023, 11, 27, 11, 5, 28, 785, DateTimeKind.Utc).AddTicks(6094), new DateTime(2023, 11, 27, 11, 5, 28, 785, DateTimeKind.Utc).AddTicks(6097) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "Rides");

            migrationBuilder.UpdateData(
                table: "CodeValues",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedDate", "EffectiveStartDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2023, 11, 23, 0, 55, 51, 64, DateTimeKind.Utc).AddTicks(9316), new DateTime(2023, 11, 23, 0, 55, 51, 64, DateTimeKind.Utc).AddTicks(9312), new DateTime(2023, 11, 23, 0, 55, 51, 64, DateTimeKind.Utc).AddTicks(9316) });

            migrationBuilder.UpdateData(
                table: "CodeValues",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedDate", "EffectiveStartDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2023, 11, 23, 0, 55, 51, 64, DateTimeKind.Utc).AddTicks(9380), new DateTime(2023, 11, 23, 0, 55, 51, 64, DateTimeKind.Utc).AddTicks(9379), new DateTime(2023, 11, 23, 0, 55, 51, 64, DateTimeKind.Utc).AddTicks(9381) });
        }
    }
}
