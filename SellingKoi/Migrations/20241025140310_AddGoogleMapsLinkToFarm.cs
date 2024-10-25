using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SellingKoi.Migrations
{
    /// <inheritdoc />
    public partial class AddGoogleMapsLinkToFarm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GoogleMapsLink",
                table: "Farms",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoogleMapsLink",
                table: "Farms");
        }
    }
}
