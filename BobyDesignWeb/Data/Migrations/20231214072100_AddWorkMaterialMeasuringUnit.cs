using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BobyDesignWeb.Data.Migrations
{
    public partial class AddWorkMaterialMeasuringUnit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WorkMaterialMeasuringUnit",
                table: "WorkMaterials",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2b89a0db-4bbc-4a80-9b30-d3df652ca902",
                column: "ConcurrencyStamp",
                value: "56e1e0d0-79b8-4c15-b55-bcbdaecbf0d0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkMaterialMeasuringUnit",
                table: "WorkMaterials");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2b89a0db-4bbc-4a80-9b30-d3df652ca902",
                column: "ConcurrencyStamp",
                value: "56e1e0d0-79b8-4c15-b559-bcbdaecbf0d0");
        }
    }
}
