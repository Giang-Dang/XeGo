using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XeGo.Services.Auth.API.Migrations
{
    /// <inheritdoc />
    public partial class addRiderTypesTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CodeValues",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedDate", "EffectiveStartDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2023, 11, 23, 9, 59, 16, 492, DateTimeKind.Utc).AddTicks(8909), new DateTime(2023, 11, 23, 9, 59, 16, 492, DateTimeKind.Utc).AddTicks(8905), new DateTime(2023, 11, 23, 9, 59, 16, 492, DateTimeKind.Utc).AddTicks(8910) });

            migrationBuilder.AddForeignKey(
                name: "FK_RiderTypes_AspNetUsers_RiderId",
                table: "RiderTypes",
                column: "RiderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiderTypes_AspNetUsers_RiderId",
                table: "RiderTypes");

            migrationBuilder.UpdateData(
                table: "CodeValues",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedDate", "EffectiveStartDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2023, 11, 23, 9, 47, 58, 946, DateTimeKind.Utc).AddTicks(2576), new DateTime(2023, 11, 23, 9, 47, 58, 946, DateTimeKind.Utc).AddTicks(2573), new DateTime(2023, 11, 23, 9, 47, 58, 946, DateTimeKind.Utc).AddTicks(2577) });
        }
    }
}
