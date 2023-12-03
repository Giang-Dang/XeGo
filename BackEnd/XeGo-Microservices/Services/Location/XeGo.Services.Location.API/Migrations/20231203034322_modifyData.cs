using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XeGo.Services.Location.API.Migrations
{
    /// <inheritdoc />
    public partial class modifyData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CodeValues",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedDate", "EffectiveStartDate", "LastModifiedDate", "Value1" },
                values: new object[] { new DateTime(2023, 12, 3, 3, 43, 21, 839, DateTimeKind.Utc).AddTicks(9065), new DateTime(2023, 12, 3, 3, 43, 21, 839, DateTimeKind.Utc).AddTicks(9061), new DateTime(2023, 12, 3, 3, 43, 21, 839, DateTimeKind.Utc).AddTicks(9066), "GEO_HASH_SQUARE_SIDE_IN_METERS" });

            migrationBuilder.UpdateData(
                table: "CodeValues",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedDate", "EffectiveStartDate", "LastModifiedDate", "Value1" },
                values: new object[] { new DateTime(2023, 12, 3, 3, 43, 21, 839, DateTimeKind.Utc).AddTicks(9070), new DateTime(2023, 12, 3, 3, 43, 21, 839, DateTimeKind.Utc).AddTicks(9068), new DateTime(2023, 12, 3, 3, 43, 21, 839, DateTimeKind.Utc).AddTicks(9071), "MAX_RADIUS_IN_METERS" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CodeValues",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedDate", "EffectiveStartDate", "LastModifiedDate", "Value1" },
                values: new object[] { new DateTime(2023, 11, 21, 9, 26, 6, 269, DateTimeKind.Utc).AddTicks(7608), new DateTime(2023, 11, 21, 9, 26, 6, 269, DateTimeKind.Utc).AddTicks(7604), new DateTime(2023, 11, 21, 9, 26, 6, 269, DateTimeKind.Utc).AddTicks(7608), "GEOHASH_SQUARE_SIDE_LENGTH_IN_METERS" });

            migrationBuilder.UpdateData(
                table: "CodeValues",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedDate", "EffectiveStartDate", "LastModifiedDate", "Value1" },
                values: new object[] { new DateTime(2023, 11, 21, 9, 26, 6, 269, DateTimeKind.Utc).AddTicks(7613), new DateTime(2023, 11, 21, 9, 26, 6, 269, DateTimeKind.Utc).AddTicks(7611), new DateTime(2023, 11, 21, 9, 26, 6, 269, DateTimeKind.Utc).AddTicks(7613), "RADIUS_IN_METERS" });
        }
    }
}
