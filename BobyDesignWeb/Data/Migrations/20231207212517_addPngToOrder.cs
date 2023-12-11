using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BobyDesignWeb.Data.Migrations
{
    public partial class addPngToOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "20641a3c-2361-477c-b6c4-f6ebdcb5c7ff");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f03bf164-9153-4f21-841e-a1d31f46e7b9");

            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6f0e689e-8a0b-4011-8197-aadb75659e3f", "be92239b-6595-4844-9c15-9bf6d86c5ee6", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fe389c99-9aaf-4b77-bb60-449d7e0e86d8", "0bb616c4-34be-41fc-a1a3-ee2165f24f8f", "Seller", "SELLER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6f0e689e-8a0b-4011-8197-aadb75659e3f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fe389c99-9aaf-4b77-bb60-449d7e0e86d8");

            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "Orders");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "20641a3c-2361-477c-b6c4-f6ebdcb5c7ff", "bd83bfe7-46f8-4d7d-99a7-563a82713a29", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f03bf164-9153-4f21-841e-a1d31f46e7b9", "36321105-7cac-48d1-8760-229144e3b168", "Seller", "SELLER" });
        }
    }
}
