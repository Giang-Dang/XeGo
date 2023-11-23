using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XeGo.Services.Ride.API.Migrations
{
    /// <inheritdoc />
    public partial class addAddressIntoRide : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DestinationAddress",
                table: "Rides",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StartAddress",
                table: "Rides",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DestinationAddress",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "StartAddress",
                table: "Rides");

            migrationBuilder.UpdateData(
                table: "CodeValues",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedDate", "EffectiveStartDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2023, 11, 22, 4, 0, 3, 554, DateTimeKind.Utc).AddTicks(7482), new DateTime(2023, 11, 22, 4, 0, 3, 554, DateTimeKind.Utc).AddTicks(7479), new DateTime(2023, 11, 22, 4, 0, 3, 554, DateTimeKind.Utc).AddTicks(7482) });

            migrationBuilder.UpdateData(
                table: "CodeValues",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedDate", "EffectiveStartDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2023, 11, 22, 4, 0, 3, 554, DateTimeKind.Utc).AddTicks(7486), new DateTime(2023, 11, 22, 4, 0, 3, 554, DateTimeKind.Utc).AddTicks(7484), new DateTime(2023, 11, 22, 4, 0, 3, 554, DateTimeKind.Utc).AddTicks(7486) });
        }
    }
}
