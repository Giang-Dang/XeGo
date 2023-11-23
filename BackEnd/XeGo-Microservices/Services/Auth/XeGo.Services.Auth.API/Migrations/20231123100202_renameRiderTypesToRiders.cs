using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XeGo.Services.Auth.API.Migrations
{
    /// <inheritdoc />
    public partial class renameRiderTypesToRiders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiderTypes");

            migrationBuilder.CreateTable(
                name: "Riders",
                columns: table => new
                {
                    RiderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Riders", x => x.RiderId);
                    table.ForeignKey(
                        name: "FK_Riders_AspNetUsers_RiderId",
                        column: x => x.RiderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "CodeValues",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedDate", "EffectiveStartDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2023, 11, 23, 10, 2, 2, 156, DateTimeKind.Utc).AddTicks(9520), new DateTime(2023, 11, 23, 10, 2, 2, 156, DateTimeKind.Utc).AddTicks(9516), new DateTime(2023, 11, 23, 10, 2, 2, 156, DateTimeKind.Utc).AddTicks(9521) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Riders");

            migrationBuilder.CreateTable(
                name: "RiderTypes",
                columns: table => new
                {
                    RiderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiderTypes", x => x.RiderId);
                    table.ForeignKey(
                        name: "FK_RiderTypes_AspNetUsers_RiderId",
                        column: x => x.RiderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "CodeValues",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedDate", "EffectiveStartDate", "LastModifiedDate" },
                values: new object[] { new DateTime(2023, 11, 23, 9, 59, 16, 492, DateTimeKind.Utc).AddTicks(8909), new DateTime(2023, 11, 23, 9, 59, 16, 492, DateTimeKind.Utc).AddTicks(8905), new DateTime(2023, 11, 23, 9, 59, 16, 492, DateTimeKind.Utc).AddTicks(8910) });
        }
    }
}
