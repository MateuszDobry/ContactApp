using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ContactApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ContactSubcategories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AddColumn<int>(
                name: "KategoriaId",
                table: "ContactSubcategories",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ContactSubcategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "KategoriaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ContactSubcategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "KategoriaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ContactSubcategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "KategoriaId",
                value: 1);

            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "Id", "DataUrodzenia", "Email", "HasloHash", "Imie", "KategoriaId", "Nazwisko", "PodkategoriaId", "Telefon" },
                values: new object[] { 4, null, "test@test.pl", "ecd71870d1963316a97e3ac3408c9835ad8cf0f3c1bc703527c30265534f75ae", "Test", 2, "Testowski", null, "111222333" });

            migrationBuilder.CreateIndex(
                name: "IX_ContactSubcategories_KategoriaId",
                table: "ContactSubcategories",
                column: "KategoriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactSubcategories_ContactCategories_KategoriaId",
                table: "ContactSubcategories",
                column: "KategoriaId",
                principalTable: "ContactCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactSubcategories_ContactCategories_KategoriaId",
                table: "ContactSubcategories");

            migrationBuilder.DropIndex(
                name: "IX_ContactSubcategories_KategoriaId",
                table: "ContactSubcategories");

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "KategoriaId",
                table: "ContactSubcategories");

            migrationBuilder.InsertData(
                table: "ContactSubcategories",
                columns: new[] { "Id", "Nazwa" },
                values: new object[] { 4, "Dostawca" });

            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "Id", "DataUrodzenia", "Email", "HasloHash", "Imie", "KategoriaId", "Nazwisko", "PodkategoriaId", "Telefon" },
                values: new object[,]
                {
                    { 1, new DateTime(1980, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "jan.kowalski@example.com", "6fb94d0cef2eca5db91338e8864febb716cdb3a9b4a4d92ff10bf33387307969", "Jan", 1, "Kowalski", 2, "123456789" },
                    { 2, new DateTime(1992, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "anna.nowak@example.com", "a60ea23572959eb7bc59d714bcc834326c0f747b008e9a5e4e500f1d23ed9225", "Anna", 2, "Nowak", null, "987654321" },
                    { 3, new DateTime(1975, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "piotr.zielinski@example.com", "8899b999ff8e33ada8c73b836bcdf91d3dbf51a3a540ac2f2914b9f69b2ca4a8", "Piotr", 3, "Zieliński", null, "555111222" }
                });
        }
    }
}
