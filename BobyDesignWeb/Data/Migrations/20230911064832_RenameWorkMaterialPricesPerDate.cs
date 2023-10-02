using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BobyDesignWeb.Data.Migrations
{
    public partial class RenameWorkMaterialPricesPerDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkMaterialPriceForDate_WorkMaterials_WorkMaterialId",
                table: "WorkMaterialPriceForDate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkMaterialPriceForDate",
                table: "WorkMaterialPriceForDate");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "356b93ea-c6ad-4fe5-a666-3d04ecf3eea8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "99647d27-a789-4e0b-9422-e4d0af34bca9");

            migrationBuilder.RenameTable(
                name: "WorkMaterialPriceForDate",
                newName: "WorkMaterialPriceForDates");

            migrationBuilder.RenameIndex(
                name: "IX_WorkMaterialPriceForDate_WorkMaterialId",
                table: "WorkMaterialPriceForDates",
                newName: "IX_WorkMaterialPriceForDates_WorkMaterialId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkMaterialPriceForDate_Date",
                table: "WorkMaterialPriceForDates",
                newName: "IX_WorkMaterialPriceForDates_Date");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkMaterialPriceForDates",
                table: "WorkMaterialPriceForDates",
                column: "WorkMaterialPriceForDateId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3bec3f4f-0d12-4ba9-bfe5-19a3141feac3", "4772ac96-17c0-4d2f-85cd-77f535bdf3a4", "Seller", "SELLER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "bfc84202-1253-48fd-bd16-088e06790c3f", "138e3ab2-aa30-4b84-bd0d-840a004149ab", "Admin", "ADMIN" });

            migrationBuilder.AddForeignKey(
                name: "FK_WorkMaterialPriceForDates_WorkMaterials_WorkMaterialId",
                table: "WorkMaterialPriceForDates",
                column: "WorkMaterialId",
                principalTable: "WorkMaterials",
                principalColumn: "WorkMaterialId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkMaterialPriceForDates_WorkMaterials_WorkMaterialId",
                table: "WorkMaterialPriceForDates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkMaterialPriceForDates",
                table: "WorkMaterialPriceForDates");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3bec3f4f-0d12-4ba9-bfe5-19a3141feac3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bfc84202-1253-48fd-bd16-088e06790c3f");

            migrationBuilder.RenameTable(
                name: "WorkMaterialPriceForDates",
                newName: "WorkMaterialPriceForDate");

            migrationBuilder.RenameIndex(
                name: "IX_WorkMaterialPriceForDates_WorkMaterialId",
                table: "WorkMaterialPriceForDate",
                newName: "IX_WorkMaterialPriceForDate_WorkMaterialId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkMaterialPriceForDates_Date",
                table: "WorkMaterialPriceForDate",
                newName: "IX_WorkMaterialPriceForDate_Date");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkMaterialPriceForDate",
                table: "WorkMaterialPriceForDate",
                column: "WorkMaterialPriceForDateId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "356b93ea-c6ad-4fe5-a666-3d04ecf3eea8", "b6278995-bdaf-4134-9a91-11bb0df09f53", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "99647d27-a789-4e0b-9422-e4d0af34bca9", "93749ee1-299c-453d-b33f-f9ad5f3e5964", "Seller", "SELLER" });

            migrationBuilder.AddForeignKey(
                name: "FK_WorkMaterialPriceForDate_WorkMaterials_WorkMaterialId",
                table: "WorkMaterialPriceForDate",
                column: "WorkMaterialId",
                principalTable: "WorkMaterials",
                principalColumn: "WorkMaterialId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
