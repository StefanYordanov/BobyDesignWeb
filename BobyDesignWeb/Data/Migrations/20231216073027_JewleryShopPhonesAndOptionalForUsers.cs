using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BobyDesignWeb.Data.Migrations
{
    public partial class JewleryShopPhonesAndOptionalForUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_JewelryShops_JewelryShopId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "JewelryShopPhoneNumbers",
                table: "JewelryShops",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "JewelryShopId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "JewelryShops",
                keyColumn: "JewelryShopId",
                keyValue: 1,
                column: "JewelryShopPhoneNumbers",
                value: "0878 306 599");

            migrationBuilder.UpdateData(
                table: "JewelryShops",
                keyColumn: "JewelryShopId",
                keyValue: 3,
                column: "JewelryShopPhoneNumbers",
                value: "0878 306 900");

            migrationBuilder.UpdateData(
                table: "JewelryShops",
                keyColumn: "JewelryShopId",
                keyValue: 4,
                column: "JewelryShopPhoneNumbers",
                value: "02/ 82 777 77, 0878 306 600");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_JewelryShops_JewelryShopId",
                table: "AspNetUsers",
                column: "JewelryShopId",
                principalTable: "JewelryShops",
                principalColumn: "JewelryShopId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_JewelryShops_JewelryShopId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "JewelryShopPhoneNumbers",
                table: "JewelryShops");

            migrationBuilder.AlterColumn<int>(
                name: "JewelryShopId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_JewelryShops_JewelryShopId",
                table: "AspNetUsers",
                column: "JewelryShopId",
                principalTable: "JewelryShops",
                principalColumn: "JewelryShopId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
