using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BobyDesignWeb.Data.Migrations
{
    public partial class depositCraftingComponents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "81cd8aaa-7ec5-42a1-9878-26d6146a0e44");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "987d4a2f-5e14-4333-b4b1-8b1b87f6373d");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeposit",
                table: "OrderCraftingComponents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2b89a0db-4bbc-4a80-9b30-d3df652ca902", "56e1e0d0-79b8-4c15-b559-bcbdaecbf0d0", "Seller", "SELLER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e2875763-6213-4636-8ab1-38d1b687abb2", "ec92d97c-486a-475c-8e12-11e26ff7b794", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2b89a0db-4bbc-4a80-9b30-d3df652ca902");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e2875763-6213-4636-8ab1-38d1b687abb2");

            migrationBuilder.DropColumn(
                name: "IsDeposit",
                table: "OrderCraftingComponents");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "81cd8aaa-7ec5-42a1-9878-26d6146a0e44", "5e2e4371-b711-47c8-ab09-c34bd237835c", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "987d4a2f-5e14-4333-b4b1-8b1b87f6373d", "afad67bd-396e-498b-b0e6-d21f259fc181", "Seller", "SELLER" });
        }
    }
}
