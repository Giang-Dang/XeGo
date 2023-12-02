using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XeGo.Services.Ride.API.Migrations
{
    /// <inheritdoc />
    public partial class changeCouponIdToDiscountId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CouponId",
                table: "Rides",
                newName: "DiscountId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DiscountId",
                table: "Rides",
                newName: "CouponId");

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
    }
}
