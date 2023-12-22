using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BobyDesignWeb.Data.Migrations
{
    public partial class JewleryShopAnnoyingIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "JewelryShops",
                keyColumn: "JewelryShopId",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "JewelryShops",
                keyColumn: "JewelryShopId",
                keyValue: 3,
                columns: new[] { "JewelryShopDescription", "JewelryShopName", "JewelryShopPhoneNumbers" },
                values: new object[] { "Цех", "Цех", "02/ 82 777 77, 0878 306 600" });

            migrationBuilder.InsertData(
                table: "JewelryShops",
                columns: new[] { "JewelryShopId", "JewelryShopDescription", "JewelryShopName", "JewelryShopPhoneNumbers" },
                values: new object[] { 2, "Бул. \"Ал. Малинов\" №75", "Младост", "0878 306 900" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "JewelryShops",
                keyColumn: "JewelryShopId",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "JewelryShops",
                keyColumn: "JewelryShopId",
                keyValue: 3,
                columns: new[] { "JewelryShopDescription", "JewelryShopName", "JewelryShopPhoneNumbers" },
                values: new object[] { "Бул. \"Ал. Малинов\" №75", "Младост", "0878 306 900" });

            migrationBuilder.InsertData(
                table: "JewelryShops",
                columns: new[] { "JewelryShopId", "JewelryShopDescription", "JewelryShopName", "JewelryShopPhoneNumbers" },
                values: new object[] { 4, "Цех", "Цех", "02/ 82 777 77, 0878 306 600" });
        }
    }
}
