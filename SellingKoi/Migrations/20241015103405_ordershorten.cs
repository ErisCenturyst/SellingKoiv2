using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SellingKoi.Migrations
{
    /// <inheritdoc />
    public partial class ordershorten : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Farms_Orders_OrderId",
                table: "Farms");

            migrationBuilder.DropIndex(
                name: "IX_Farms_OrderId",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Farms");

            migrationBuilder.CreateTable(
                name: "OrtherShortens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Registration_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrtherShortens", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrtherShortens");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "Farms",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Farms_OrderId",
                table: "Farms",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Farms_Orders_OrderId",
                table: "Farms",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
