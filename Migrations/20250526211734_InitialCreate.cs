using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ContactApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nazwa = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactSubcategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nazwa = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactSubcategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Imie = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    HasloHash = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Imie = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Nazwisko = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    HasloHash = table.Column<string>(type: "TEXT", nullable: false),
                    KategoriaId = table.Column<int>(type: "INTEGER", nullable: false),
                    PodkategoriaId = table.Column<int>(type: "INTEGER", nullable: true),
                    Telefon = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    DataUrodzenia = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contacts_ContactCategories_KategoriaId",
                        column: x => x.KategoriaId,
                        principalTable: "ContactCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contacts_ContactSubcategories_PodkategoriaId",
                        column: x => x.PodkategoriaId,
                        principalTable: "ContactSubcategories",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "ContactCategories",
                columns: new[] { "Id", "Nazwa" },
                values: new object[,]
                {
                    { 1, "Służbowy" },
                    { 2, "Prywatny" },
                    { 3, "Inny" }
                });

            migrationBuilder.InsertData(
                table: "ContactSubcategories",
                columns: new[] { "Id", "Nazwa" },
                values: new object[,]
                {
                    { 1, "Szef" },
                    { 2, "Klient" },
                    { 3, "Współpracownik" },
                    { 4, "Dostawca" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "HasloHash", "Imie" },
                values: new object[] { 1, "jan@example.com", "haslo123", "Jan" });

            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "Id", "DataUrodzenia", "Email", "HasloHash", "Imie", "KategoriaId", "Nazwisko", "PodkategoriaId", "Telefon" },
                values: new object[,]
                {
                    { 1, new DateTime(1980, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "jan.kowalski@example.com", "haslo123", "Jan", 1, "Kowalski", 2, "123456789" },
                    { 2, new DateTime(1992, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "anna.nowak@example.com", "haslo456", "Anna", 2, "Nowak", null, "987654321" },
                    { 3, new DateTime(1975, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "piotr.zielinski@example.com", "haslo789", "Piotr", 3, "Zieliński", null, "555111222" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_Email",
                table: "Contacts",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_KategoriaId",
                table: "Contacts",
                column: "KategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_PodkategoriaId",
                table: "Contacts",
                column: "PodkategoriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ContactCategories");

            migrationBuilder.DropTable(
                name: "ContactSubcategories");
        }
    }
}
