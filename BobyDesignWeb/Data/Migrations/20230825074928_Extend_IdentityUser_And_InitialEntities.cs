using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BobyDesignWeb.Data.Migrations
{
    public partial class Extend_IdentityUser_And_InitialEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "JewelryShop",
                columns: table => new
                {
                    JewelryShopId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JewelryShopName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    JewelryShopDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JewelryShop", x => x.JewelryShopId);
                });

            migrationBuilder.CreateTable(
                name: "WorkMaterial",
                columns: table => new
                {
                    WorkMaterialId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkMaterialName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkMaterial", x => x.WorkMaterialId);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    JewelryShopId = table.Column<int>(type: "int", nullable: false),
                    OrderDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OrderCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinishingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LaborPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Deposit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShopUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NotifyCustomer = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Order_AspNetUsers_ShopUserId",
                        column: x => x.ShopUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_JewelryShop_JewelryShopId",
                        column: x => x.JewelryShopId,
                        principalTable: "JewelryShop",
                        principalColumn: "JewelryShopId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkMaterialPriceForDate",
                columns: table => new
                {
                    WorkMaterialPriceForDateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkMaterialId = table.Column<int>(type: "int", nullable: false),
                    SellingPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchasingPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkMaterialPriceForDate", x => x.WorkMaterialPriceForDateId);
                    table.ForeignKey(
                        name: "FK_WorkMaterialPriceForDate_WorkMaterial_WorkMaterialId",
                        column: x => x.WorkMaterialId,
                        principalTable: "WorkMaterial",
                        principalColumn: "WorkMaterialId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderCraftingComponent",
                columns: table => new
                {
                    OrderCraftingComponentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkMaterialId = table.Column<int>(type: "int", nullable: false),
                    WorkMaterialPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    WorkMaterialQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderCraftingComponent", x => x.OrderCraftingComponentId);
                    table.ForeignKey(
                        name: "FK_OrderCraftingComponent_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderCraftingComponent_WorkMaterial_WorkMaterialId",
                        column: x => x.WorkMaterialId,
                        principalTable: "WorkMaterial",
                        principalColumn: "WorkMaterialId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_CustomerId",
                table: "Order",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_JewelryShopId",
                table: "Order",
                column: "JewelryShopId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ShopUserId",
                table: "Order",
                column: "ShopUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderCraftingComponent_OrderId",
                table: "OrderCraftingComponent",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderCraftingComponent_WorkMaterialId",
                table: "OrderCraftingComponent",
                column: "WorkMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkMaterialPriceForDate_Date",
                table: "WorkMaterialPriceForDate",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_WorkMaterialPriceForDate_WorkMaterialId",
                table: "WorkMaterialPriceForDate",
                column: "WorkMaterialId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderCraftingComponent");

            migrationBuilder.DropTable(
                name: "WorkMaterialPriceForDate");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "WorkMaterial");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "JewelryShop");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");
        }
    }
}
