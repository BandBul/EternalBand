using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EternalBAND.Data.Migrations
{
    /// <inheritdoc />
    public partial class RelatedElement_Addition_For_Notification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RelatedElementId",
                table: "Notification",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "39d321fc-0911-4412-a19e-98fb7d068440",
                column: "NormalizedName",
                value: "ADMIN");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a3641119-ff91-4eca-aa32-120c36d61d1a",
                column: "NormalizedName",
                value: "USER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RelatedElementId",
                table: "Notification");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "39d321fc-0911-4412-a19e-98fb7d068440",
                column: "NormalizedName",
                value: "Admin");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a3641119-ff91-4eca-aa32-120c36d61d1a",
                column: "NormalizedName",
                value: "User");
        }
    }
}
