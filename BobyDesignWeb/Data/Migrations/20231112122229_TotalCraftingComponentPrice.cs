using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BobyDesignWeb.Data.Migrations
{
    public partial class TotalCraftingComponentPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "089859fa-9bd1-4b2a-93f1-119c9a7b0162");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "92e1461b-1f75-4b34-8c36-8d66926b552d");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalComponentPrice",
                table: "OrderCraftingComponents",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "20641a3c-2361-477c-b6c4-f6ebdcb5c7ff", "bd83bfe7-46f8-4d7d-99a7-563a82713a29", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f03bf164-9153-4f21-841e-a1d31f46e7b9", "36321105-7cac-48d1-8760-229144e3b168", "Seller", "SELLER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "20641a3c-2361-477c-b6c4-f6ebdcb5c7ff");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f03bf164-9153-4f21-841e-a1d31f46e7b9");

            migrationBuilder.DropColumn(
                name: "TotalComponentPrice",
                table: "OrderCraftingComponents");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "089859fa-9bd1-4b2a-93f1-119c9a7b0162", "f2951417-0dca-4e45-81e2-0e493641dcb1", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "92e1461b-1f75-4b34-8c36-8d66926b552d", "01e27c56-c3e3-4b43-991a-fe4ed22c8ca5", "Seller", "SELLER" });
        }
    }
}
