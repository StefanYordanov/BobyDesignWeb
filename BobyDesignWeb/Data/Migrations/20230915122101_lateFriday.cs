using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BobyDesignWeb.Data.Migrations
{
    public partial class lateFriday : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3bec3f4f-0d12-4ba9-bfe5-19a3141feac3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bfc84202-1253-48fd-bd16-088e06790c3f");

            migrationBuilder.AddColumn<int>(
                name: "WorkMaterialPricingType",
                table: "WorkMaterials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "089859fa-9bd1-4b2a-93f1-119c9a7b0162", "f2951417-0dca-4e45-81e2-0e493641dcb1", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "92e1461b-1f75-4b34-8c36-8d66926b552d", "01e27c56-c3e3-4b43-991a-fe4ed22c8ca5", "Seller", "SELLER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "089859fa-9bd1-4b2a-93f1-119c9a7b0162");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "92e1461b-1f75-4b34-8c36-8d66926b552d");

            migrationBuilder.DropColumn(
                name: "WorkMaterialPricingType",
                table: "WorkMaterials");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3bec3f4f-0d12-4ba9-bfe5-19a3141feac3", "4772ac96-17c0-4d2f-85cd-77f535bdf3a4", "Seller", "SELLER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "bfc84202-1253-48fd-bd16-088e06790c3f", "138e3ab2-aa30-4b84-bd0d-840a004149ab", "Admin", "ADMIN" });
        }
    }
}
