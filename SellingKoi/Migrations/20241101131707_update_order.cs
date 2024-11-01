using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SellingKoi.Migrations
{
    /// <inheritdoc />
    public partial class update_order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "participants",
                table: "OrtherShortens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "participantsPhone",
                table: "OrtherShortens",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "participants",
                table: "OrtherShortens");

            migrationBuilder.DropColumn(
                name: "participantsPhone",
                table: "OrtherShortens");
        }
    }
}
