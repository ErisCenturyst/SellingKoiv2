using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SellingKoi.Migrations
{
    /// <inheritdoc />
    public partial class dateofwishdepature_ordershortenv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateOfWishDepature",
                table: "OrtherShortens",
                newName: "DepartureTo");

            migrationBuilder.AddColumn<DateTime>(
                name: "DepartureFrom",
                table: "OrtherShortens",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartureFrom",
                table: "OrtherShortens");

            migrationBuilder.RenameColumn(
                name: "DepartureTo",
                table: "OrtherShortens",
                newName: "DateOfWishDepature");
        }
    }
}
