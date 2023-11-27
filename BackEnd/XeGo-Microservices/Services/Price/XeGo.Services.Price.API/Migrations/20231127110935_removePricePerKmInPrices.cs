using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XeGo.Services.Price.API.Migrations
{
    /// <inheritdoc />
    public partial class removePricePerKmInPrices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerKm",
                table: "Prices");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PricePerKm",
                table: "Prices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
