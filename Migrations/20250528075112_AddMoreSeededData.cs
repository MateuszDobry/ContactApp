using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ContactApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreSeededData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "Id", "DataUrodzenia", "Email", "HasloHash", "Imie", "KategoriaId", "Nazwisko", "PodkategoriaId", "Telefon" },
                values: new object[,]
                {
                    { 5, new DateTime(1990, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "anna.kowalska@example.com", "fbb4a8a163ffa958b4f02bf9cabb30cfefb40de803f2c4c346a9d39b3be1b544", "Anna", 1, "Kowalska", 2, "444555666" },
                    { 6, null, "piotr.nowak@email.com", "e8b97e11e6240b2bac77fcd9d654495475cfe6e2cdfe6a0c8e6619f90c98080e", "Piotr", 3, "Nowak", 1, "777888999" },
                    { 7, new DateTime(1985, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "zofia.wojcik@mail.pl", "5421cfd52b4989d7e464d3a13a52626a9b64c3016baec6f2b7df90f0c13e913c", "Zofia", 2, "Wojcik", null, "123456789" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
