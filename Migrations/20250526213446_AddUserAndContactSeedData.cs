using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAndContactSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 1,
                column: "HasloHash",
                value: "6fb94d0cef2eca5db91338e8864febb716cdb3a9b4a4d92ff10bf33387307969");

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 2,
                column: "HasloHash",
                value: "a60ea23572959eb7bc59d714bcc834326c0f747b008e9a5e4e500f1d23ed9225");

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 3,
                column: "HasloHash",
                value: "8899b999ff8e33ada8c73b836bcdf91d3dbf51a3a540ac2f2914b9f69b2ca4a8");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "HasloHash",
                value: "a15f8ae07675bfb96e084bfb4f52fb2c22091061aae86e0eb76a55f4e52dd74e");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 1,
                column: "HasloHash",
                value: "haslo123");

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 2,
                column: "HasloHash",
                value: "haslo456");

            migrationBuilder.UpdateData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 3,
                column: "HasloHash",
                value: "haslo789");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "HasloHash",
                value: "haslo123");
        }
    }
}
