using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XeGo.Services.CodeValue.API.Migrations
{
    /// <inheritdoc />
    public partial class modifyCodeValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CodeMetaData",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_CodeMetaData_Name",
                table: "CodeMetaData",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CodeMetaData_Name",
                table: "CodeMetaData");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CodeMetaData",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
