using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace XeGo.Services.Price.API.Migrations
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
                name: "Discounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Percent = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    FromDay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleTypePrices",
                columns: table => new
                {
                    VehicleTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PricePerKm = table.Column<double>(type: "float", nullable: false),
                    DropCharge = table.Column<double>(type: "float", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleTypePrices", x => x.VehicleTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    RideId = table.Column<int>(type: "int", nullable: false),
                    DiscountId = table.Column<int>(type: "int", nullable: true),
                    VehicleTypeId = table.Column<int>(type: "int", nullable: false),
                    DistanceInMeters = table.Column<double>(type: "float", nullable: false),
                    TotalPrice = table.Column<double>(type: "float", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.RideId);
                    table.ForeignKey(
                        name: "FK_Prices_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Prices_VehicleTypePrices_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalTable: "VehicleTypePrices",
                        principalColumn: "VehicleTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CodeValues",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "EffectiveEndDate", "EffectiveStartDate", "IsActive", "LastModifiedBy", "LastModifiedDate", "Name", "SortOrder", "Value1", "Value10", "Value10Type", "Value1Type", "Value2", "Value2Type", "Value3", "Value3Type", "Value4", "Value4Type", "Value5", "Value5Type", "Value6", "Value6Type", "Value7", "Value7Type", "Value8", "Value8Type", "Value9", "Value9Type" },
                values: new object[] { 1, "", new DateTime(2023, 12, 9, 2, 47, 44, 599, DateTimeKind.Utc).AddTicks(8789), new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), new DateTime(2023, 12, 9, 2, 47, 44, 599, DateTimeKind.Utc).AddTicks(8790), true, "", new DateTime(2023, 12, 9, 2, 47, 44, 599, DateTimeKind.Utc).AddTicks(8789), "DROP_CHARGE_THRESHOLD", null, "500", null, null, "DOUBLE", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null });

            migrationBuilder.InsertData(
                table: "VehicleTypePrices",
                columns: new[] { "VehicleTypeId", "CreatedBy", "CreatedDate", "DropCharge", "LastModifiedBy", "LastModifiedDate", "PricePerKm" },
                values: new object[,]
                {
                    { 1, "SYSTEM", new DateTime(2023, 12, 9, 2, 47, 44, 599, DateTimeKind.Utc).AddTicks(8650), 1.0, "SYSTEM", new DateTime(2023, 12, 9, 2, 47, 44, 599, DateTimeKind.Utc).AddTicks(8650), 1.0 },
                    { 2, "SYSTEM", new DateTime(2023, 12, 9, 2, 47, 44, 599, DateTimeKind.Utc).AddTicks(8653), 1.5, "SYSTEM", new DateTime(2023, 12, 9, 2, 47, 44, 599, DateTimeKind.Utc).AddTicks(8653), 1.5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prices_DiscountId",
                table: "Prices",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_VehicleTypeId",
                table: "Prices",
                column: "VehicleTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodeValues");

            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "VehicleTypePrices");
        }
    }
}
