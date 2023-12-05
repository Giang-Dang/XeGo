using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace XeGo.Services.Auth.API.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedDataToIdentiyRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "03683eb7-86a1-4c03-b5a7-113f01a36359", null, "Rider", "RIDER" },
                    { "a29aead9-eca7-4dd9-b202-b0f5a644682d", null, "Driver", "DRIVER" },
                    { "d29c61c1-13b0-4778-80d2-be3c7f5fc607", null, "Staff", "STAFF" },
                    { "f6871a0e-e227-44fe-9965-0f82e9c66254", null, "Admin", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "CodeValues",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedDate", "EffectiveStartDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2023, 12, 3, 8, 43, 19, 333, DateTimeKind.Utc).AddTicks(7594), new DateTime(2023, 12, 3, 8, 43, 19, 333, DateTimeKind.Utc).AddTicks(7590), new DateTime(2023, 12, 3, 8, 43, 19, 333, DateTimeKind.Utc).AddTicks(7596) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "03683eb7-86a1-4c03-b5a7-113f01a36359");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a29aead9-eca7-4dd9-b202-b0f5a644682d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d29c61c1-13b0-4778-80d2-be3c7f5fc607");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f6871a0e-e227-44fe-9965-0f82e9c66254");

            migrationBuilder.UpdateData(
                table: "CodeValues",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedDate", "EffectiveStartDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2023, 11, 23, 10, 2, 2, 156, DateTimeKind.Utc).AddTicks(9520), new DateTime(2023, 11, 23, 10, 2, 2, 156, DateTimeKind.Utc).AddTicks(9516), new DateTime(2023, 11, 23, 10, 2, 2, 156, DateTimeKind.Utc).AddTicks(9521) });
        }
    }
}
