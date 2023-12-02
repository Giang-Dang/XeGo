using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XeGo.Services.Ride.API.Migrations
{
    /// <inheritdoc />
    public partial class addVehicleTypeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "VehicleId",
                table: "Rides",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "VehicleTypeId",
                table: "Rides",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "CodeValues",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedDate", "EffectiveStartDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2023, 12, 2, 4, 50, 54, 773, DateTimeKind.Utc).AddTicks(6867), new DateTime(2023, 12, 2, 4, 50, 54, 773, DateTimeKind.Utc).AddTicks(6863), new DateTime(2023, 12, 2, 4, 50, 54, 773, DateTimeKind.Utc).AddTicks(6867) });

            migrationBuilder.UpdateData(
                table: "CodeValues",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedDate", "EffectiveStartDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2023, 12, 2, 4, 50, 54, 773, DateTimeKind.Utc).AddTicks(6872), new DateTime(2023, 12, 2, 4, 50, 54, 773, DateTimeKind.Utc).AddTicks(6870), new DateTime(2023, 12, 2, 4, 50, 54, 773, DateTimeKind.Utc).AddTicks(6872) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VehicleTypeId",
                table: "Rides");

            migrationBuilder.AlterColumn<int>(
                name: "VehicleId",
                table: "Rides",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "CodeValues",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedDate", "EffectiveStartDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2023, 12, 2, 3, 9, 18, 417, DateTimeKind.Utc).AddTicks(1956), new DateTime(2023, 12, 2, 3, 9, 18, 417, DateTimeKind.Utc).AddTicks(1952), new DateTime(2023, 12, 2, 3, 9, 18, 417, DateTimeKind.Utc).AddTicks(1957) });

            migrationBuilder.UpdateData(
                table: "CodeValues",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedDate", "EffectiveStartDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2023, 12, 2, 3, 9, 18, 417, DateTimeKind.Utc).AddTicks(1961), new DateTime(2023, 12, 2, 3, 9, 18, 417, DateTimeKind.Utc).AddTicks(1959), new DateTime(2023, 12, 2, 3, 9, 18, 417, DateTimeKind.Utc).AddTicks(1962) });
        }
    }
}
