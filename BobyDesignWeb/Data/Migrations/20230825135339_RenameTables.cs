using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BobyDesignWeb.Data.Migrations
{
    public partial class RenameTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_ShopUserId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Customer_CustomerId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_JewelryShop_JewelryShopId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderCraftingComponent_Order_OrderId",
                table: "OrderCraftingComponent");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderCraftingComponent_WorkMaterial_WorkMaterialId",
                table: "OrderCraftingComponent");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkMaterialPriceForDate_WorkMaterial_WorkMaterialId",
                table: "WorkMaterialPriceForDate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkMaterial",
                table: "WorkMaterial");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderCraftingComponent",
                table: "OrderCraftingComponent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JewelryShop",
                table: "JewelryShop");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customer",
                table: "Customer");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "befeecd7-e4b5-4fe1-b217-6c93b4e9e838");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb5c54b0-b2ea-484e-8108-e690e55bdcc7");

            migrationBuilder.RenameTable(
                name: "WorkMaterial",
                newName: "WorkMaterials");

            migrationBuilder.RenameTable(
                name: "OrderCraftingComponent",
                newName: "OrderCraftingComponents");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "JewelryShop",
                newName: "JewelryShops");

            migrationBuilder.RenameTable(
                name: "Customer",
                newName: "Customers");

            migrationBuilder.RenameIndex(
                name: "IX_OrderCraftingComponent_WorkMaterialId",
                table: "OrderCraftingComponents",
                newName: "IX_OrderCraftingComponents_WorkMaterialId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderCraftingComponent_OrderId",
                table: "OrderCraftingComponents",
                newName: "IX_OrderCraftingComponents_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_ShopUserId",
                table: "Orders",
                newName: "IX_Orders_ShopUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_JewelryShopId",
                table: "Orders",
                newName: "IX_Orders_JewelryShopId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_CustomerId",
                table: "Orders",
                newName: "IX_Orders_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkMaterials",
                table: "WorkMaterials",
                column: "WorkMaterialId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderCraftingComponents",
                table: "OrderCraftingComponents",
                column: "OrderCraftingComponentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JewelryShops",
                table: "JewelryShops",
                column: "JewelryShopId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "CustomerId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "356b93ea-c6ad-4fe5-a666-3d04ecf3eea8", "b6278995-bdaf-4134-9a91-11bb0df09f53", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "99647d27-a789-4e0b-9422-e4d0af34bca9", "93749ee1-299c-453d-b33f-f9ad5f3e5964", "Seller", "SELLER" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderCraftingComponents_Orders_OrderId",
                table: "OrderCraftingComponents",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderCraftingComponents_WorkMaterials_WorkMaterialId",
                table: "OrderCraftingComponents",
                column: "WorkMaterialId",
                principalTable: "WorkMaterials",
                principalColumn: "WorkMaterialId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_ShopUserId",
                table: "Orders",
                column: "ShopUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_JewelryShops_JewelryShopId",
                table: "Orders",
                column: "JewelryShopId",
                principalTable: "JewelryShops",
                principalColumn: "JewelryShopId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkMaterialPriceForDate_WorkMaterials_WorkMaterialId",
                table: "WorkMaterialPriceForDate",
                column: "WorkMaterialId",
                principalTable: "WorkMaterials",
                principalColumn: "WorkMaterialId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderCraftingComponents_Orders_OrderId",
                table: "OrderCraftingComponents");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderCraftingComponents_WorkMaterials_WorkMaterialId",
                table: "OrderCraftingComponents");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_ShopUserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_JewelryShops_JewelryShopId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkMaterialPriceForDate_WorkMaterials_WorkMaterialId",
                table: "WorkMaterialPriceForDate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkMaterials",
                table: "WorkMaterials");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderCraftingComponents",
                table: "OrderCraftingComponents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JewelryShops",
                table: "JewelryShops");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "356b93ea-c6ad-4fe5-a666-3d04ecf3eea8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "99647d27-a789-4e0b-9422-e4d0af34bca9");

            migrationBuilder.RenameTable(
                name: "WorkMaterials",
                newName: "WorkMaterial");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Order");

            migrationBuilder.RenameTable(
                name: "OrderCraftingComponents",
                newName: "OrderCraftingComponent");

            migrationBuilder.RenameTable(
                name: "JewelryShops",
                newName: "JewelryShop");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Customer");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ShopUserId",
                table: "Order",
                newName: "IX_Order_ShopUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_JewelryShopId",
                table: "Order",
                newName: "IX_Order_JewelryShopId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CustomerId",
                table: "Order",
                newName: "IX_Order_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderCraftingComponents_WorkMaterialId",
                table: "OrderCraftingComponent",
                newName: "IX_OrderCraftingComponent_WorkMaterialId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderCraftingComponents_OrderId",
                table: "OrderCraftingComponent",
                newName: "IX_OrderCraftingComponent_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkMaterial",
                table: "WorkMaterial",
                column: "WorkMaterialId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderCraftingComponent",
                table: "OrderCraftingComponent",
                column: "OrderCraftingComponentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JewelryShop",
                table: "JewelryShop",
                column: "JewelryShopId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customer",
                table: "Customer",
                column: "CustomerId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "befeecd7-e4b5-4fe1-b217-6c93b4e9e838", "70f4f5ba-58dd-441b-a4be-7094991518e2", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fb5c54b0-b2ea-484e-8108-e690e55bdcc7", "556c5312-6d87-42fd-871e-99aa33a445f7", "Seller", "SELLER" });

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_ShopUserId",
                table: "Order",
                column: "ShopUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Customer_CustomerId",
                table: "Order",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_JewelryShop_JewelryShopId",
                table: "Order",
                column: "JewelryShopId",
                principalTable: "JewelryShop",
                principalColumn: "JewelryShopId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderCraftingComponent_Order_OrderId",
                table: "OrderCraftingComponent",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderCraftingComponent_WorkMaterial_WorkMaterialId",
                table: "OrderCraftingComponent",
                column: "WorkMaterialId",
                principalTable: "WorkMaterial",
                principalColumn: "WorkMaterialId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkMaterialPriceForDate_WorkMaterial_WorkMaterialId",
                table: "WorkMaterialPriceForDate",
                column: "WorkMaterialId",
                principalTable: "WorkMaterial",
                principalColumn: "WorkMaterialId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
