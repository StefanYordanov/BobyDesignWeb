using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BobyDesignWeb.Migrations
{
    public partial class linkedorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LinkedOrderId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_LinkedOrderId",
                table: "Orders",
                column: "LinkedOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Orders_LinkedOrderId",
                table: "Orders",
                column: "LinkedOrderId",
                principalTable: "Orders",
                principalColumn: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Orders_LinkedOrderId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_LinkedOrderId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "LinkedOrderId",
                table: "Orders");
        }
    }
}
