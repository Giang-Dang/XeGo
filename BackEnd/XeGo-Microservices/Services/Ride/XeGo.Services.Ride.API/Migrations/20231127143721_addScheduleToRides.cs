using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XeGo.Services.Ride.API.Migrations
{
    /// <inheritdoc />
    public partial class addScheduleToRides : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsScheduleRide",
                table: "Rides",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PickupTime",
                table: "Rides",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "CodeValues",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedDate", "EffectiveStartDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2023, 11, 27, 14, 37, 21, 143, DateTimeKind.Utc).AddTicks(6216), new DateTime(2023, 11, 27, 14, 37, 21, 143, DateTimeKind.Utc).AddTicks(6213), new DateTime(2023, 11, 27, 14, 37, 21, 143, DateTimeKind.Utc).AddTicks(6216) });

            migrationBuilder.UpdateData(
                table: "CodeValues",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedDate", "EffectiveStartDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2023, 11, 27, 14, 37, 21, 143, DateTimeKind.Utc).AddTicks(6220), new DateTime(2023, 11, 27, 14, 37, 21, 143, DateTimeKind.Utc).AddTicks(6219), new DateTime(2023, 11, 27, 14, 37, 21, 143, DateTimeKind.Utc).AddTicks(6221) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsScheduleRide",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "PickupTime",
                table: "Rides");

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
    }
}
