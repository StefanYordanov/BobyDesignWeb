using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BobyDesignWeb.Data.Migrations
{
    public partial class AddUserJewleryShop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JewelryShopId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "JewelryShops",
                columns: new[] { "JewelryShopId", "JewelryShopDescription", "JewelryShopName" },
                values: new object[] { 1, "Ул. \"Обелско Шосе\" №20", "София, Хипермаркет HIT Люлин" });

            migrationBuilder.InsertData(
                table: "JewelryShops",
                columns: new[] { "JewelryShopId", "JewelryShopDescription", "JewelryShopName" },
                values: new object[] { 3, "Бул. \"Ал. Малинов\" №75", "София, Хипермаркет HIT Младост" });

            migrationBuilder.InsertData(
                table: "JewelryShops",
                columns: new[] { "JewelryShopId", "JewelryShopDescription", "JewelryShopName" },
                values: new object[] { 4, "Цех", "София, Цех" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_JewelryShopId",
                table: "AspNetUsers",
                column: "JewelryShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_JewelryShops_JewelryShopId",
                table: "AspNetUsers",
                column: "JewelryShopId",
                principalTable: "JewelryShops",
                principalColumn: "JewelryShopId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_JewelryShops_JewelryShopId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_JewelryShopId",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "JewelryShops",
                keyColumn: "JewelryShopId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "JewelryShops",
                keyColumn: "JewelryShopId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "JewelryShops",
                keyColumn: "JewelryShopId",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "JewelryShopId",
                table: "AspNetUsers");
        }
    }
}
