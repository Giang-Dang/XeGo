using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace XeGo.Services.Ride.API.Migrations
{
    /// <inheritdoc />
    public partial class initialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CodeValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    EffectiveStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Value1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value1Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Value2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Value2Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Value3 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Value3Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Value4 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Value4Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Value5 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Value5Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Value6 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Value6Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Value7 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Value7Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Value8 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Value8Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Value9 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Value9Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Value10 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Value10Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeValues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rides",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RiderId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DriverId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CouponId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartLatitude = table.Column<double>(type: "float", nullable: false),
                    StartLongitude = table.Column<double>(type: "float", nullable: false),
                    DestinationLatitude = table.Column<double>(type: "float", nullable: false),
                    DestinationLongitude = table.Column<double>(type: "float", nullable: false),
                    CancelledBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CancellationReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rides", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserConnectionIds",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConnectionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserConnectionIds", x => x.UserId);
                });

            migrationBuilder.InsertData(
                table: "CodeValues",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "EffectiveEndDate", "EffectiveStartDate", "IsActive", "LastModifiedBy", "LastModifiedDate", "Name", "SortOrder", "Value1", "Value10", "Value10Type", "Value1Type", "Value2", "Value2Type", "Value3", "Value3Type", "Value4", "Value4Type", "Value5", "Value5Type", "Value6", "Value6Type", "Value7", "Value7Type", "Value8", "Value8Type", "Value9", "Value9Type" },
                values: new object[,]
                {
                    { 11, "ADMIN", new DateTime(2023, 11, 22, 4, 0, 3, 554, DateTimeKind.Utc).AddTicks(7482), new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), new DateTime(2023, 11, 22, 4, 0, 3, 554, DateTimeKind.Utc).AddTicks(7479), true, "ADMIN", new DateTime(2023, 11, 22, 4, 0, 3, 554, DateTimeKind.Utc).AddTicks(7482), "GEOHASH", 1, "GEO_HASH_SQUARE_SIDE_IN_METERS", null, null, "STRING", "500", "DOUBLE", null, null, null, null, null, null, null, null, null, null, null, null, null, null },
                    { 12, "ADMIN", new DateTime(2023, 11, 22, 4, 0, 3, 554, DateTimeKind.Utc).AddTicks(7486), new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), new DateTime(2023, 11, 22, 4, 0, 3, 554, DateTimeKind.Utc).AddTicks(7484), true, "ADMIN", new DateTime(2023, 11, 22, 4, 0, 3, 554, DateTimeKind.Utc).AddTicks(7486), "GEOHASH", 1, "MAX_RADIUS_IN_METERS", null, null, "STRING", "1000", "DOUBLE", null, null, null, null, null, null, null, null, null, null, null, null, null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodeValues");

            migrationBuilder.DropTable(
                name: "Rides");

            migrationBuilder.DropTable(
                name: "UserConnectionIds");
        }
    }
}
