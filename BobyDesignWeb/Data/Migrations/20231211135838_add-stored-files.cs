using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BobyDesignWeb.Data.Migrations
{
    public partial class addstoredfiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6f0e689e-8a0b-4011-8197-aadb75659e3f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fe389c99-9aaf-4b77-bb60-449d7e0e86d8");

            migrationBuilder.AddColumn<int>(
                name: "StoredFileId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StoredFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoredFiles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "81cd8aaa-7ec5-42a1-9878-26d6146a0e44", "5e2e4371-b711-47c8-ab09-c34bd237835c", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "987d4a2f-5e14-4333-b4b1-8b1b87f6373d", "afad67bd-396e-498b-b0e6-d21f259fc181", "Seller", "SELLER" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StoredFileId",
                table: "Orders",
                column: "StoredFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_StoredFiles_StoredFileId",
                table: "Orders",
                column: "StoredFileId",
                principalTable: "StoredFiles",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_StoredFiles_StoredFileId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "StoredFiles");

            migrationBuilder.DropIndex(
                name: "IX_Orders_StoredFileId",
                table: "Orders");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "81cd8aaa-7ec5-42a1-9878-26d6146a0e44");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "987d4a2f-5e14-4333-b4b1-8b1b87f6373d");

            migrationBuilder.DropColumn(
                name: "StoredFileId",
                table: "Orders");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6f0e689e-8a0b-4011-8197-aadb75659e3f", "be92239b-6595-4844-9c15-9bf6d86c5ee6", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fe389c99-9aaf-4b77-bb60-449d7e0e86d8", "0bb616c4-34be-41fc-a1a3-ee2165f24f8f", "Seller", "SELLER" });
        }
    }
}
