using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XeGo.Services.CodeValue.API.Migrations
{
    /// <inheritdoc />
    public partial class initialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CodeMetaData",
                columns: table => new
                {
                    RowId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LongDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    Value1Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value1Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value2Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value2Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value3Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value3Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value4Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value4Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value5Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value5Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value6Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value6Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value7Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value7Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value8Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value8Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value9Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value9Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value10Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value10Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeMetaData", x => x.RowId);
                });

            migrationBuilder.CreateTable(
                name: "CodeValues",
                columns: table => new
                {
                    RowId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShortDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LongDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    EffectiveStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Value1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Value2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Value3 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Value4 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Value5 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Value6 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Value7 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Value8 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Value9 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Value10 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeValues", x => x.RowId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CodeValues_Name",
                table: "CodeValues",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodeMetaData");

            migrationBuilder.DropTable(
                name: "CodeValues");
        }
    }
}
