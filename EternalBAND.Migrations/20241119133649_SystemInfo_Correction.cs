using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EternalBAND.Migrations
{
    /// <inheritdoc />
    public partial class SystemInfo_Correction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SystemInfo",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "SystemInfo",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "SystemInfo",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "248feb45-0294-450c-afd2-9c6de89023b9",
                column: "PhotoPath",
                value: "/images/user_photo/profile.png");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "248feb45-0294-450c-afd2-9c6de89023b9",
                column: "PhotoPath",
                value: "/img/user_photo/profile.png");

            migrationBuilder.InsertData(
                table: "SystemInfo",
                columns: new[] { "Id", "Desc", "Type", "Value" },
                values: new object[,]
                {
                    { 13, "Site Logo", "site-logo", "/images/logo-light.png" },
                    { 14, "Site Favicon", "site-favicon", "/images/favicon.ico" },
                    { 15, "Site Domain", "site-domain", "https://bandbul.checktheproject.com" }
                });
        }
    }
}
