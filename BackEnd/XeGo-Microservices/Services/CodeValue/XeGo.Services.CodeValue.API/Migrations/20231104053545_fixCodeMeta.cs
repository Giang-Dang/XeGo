using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XeGo.Services.CodeValue.API.Migrations
{
    /// <inheritdoc />
    public partial class fixCodeMeta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "CodeMetaData");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "CodeMetaData",
                type: "int",
                nullable: true);
        }
    }
}
