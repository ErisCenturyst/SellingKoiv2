using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SellingKoi.Migrations
{
    /// <inheritdoc />
    public partial class ordershorten_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "koisid",
                table: "OrtherShortens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "koisname",
                table: "OrtherShortens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "routeid",
                table: "OrtherShortens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "routename",
                table: "OrtherShortens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "koisid",
                table: "OrtherShortens");

            migrationBuilder.DropColumn(
                name: "koisname",
                table: "OrtherShortens");

            migrationBuilder.DropColumn(
                name: "routeid",
                table: "OrtherShortens");

            migrationBuilder.DropColumn(
                name: "routename",
                table: "OrtherShortens");
        }
    }
}
