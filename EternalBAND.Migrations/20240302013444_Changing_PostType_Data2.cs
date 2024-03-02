using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EternalBAND.Migrations
{
    /// <inheritdoc />
    public partial class Changing_PostType_Data2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TypeText",
                table: "PostTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AddedDate", "TypeText" },
                values: new object[] { new DateTime(2024, 3, 2, 2, 34, 43, 806, DateTimeKind.Local).AddTicks(82), "Müzisyen İlanları" });

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AddedDate", "TypeText" },
                values: new object[] { new DateTime(2024, 3, 2, 2, 34, 43, 806, DateTimeKind.Local).AddTicks(134), "Grup İlanları" });

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AddedDate", "TypeText" },
                values: new object[] { new DateTime(2024, 3, 2, 2, 34, 43, 806, DateTimeKind.Local).AddTicks(138), "Ders İlanları" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeText",
                table: "PostTypes");

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "AddedDate",
                value: new DateTime(2024, 3, 2, 1, 38, 55, 78, DateTimeKind.Local).AddTicks(5732));

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "AddedDate",
                value: new DateTime(2024, 3, 2, 1, 38, 55, 78, DateTimeKind.Local).AddTicks(5787));

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "AddedDate",
                value: new DateTime(2024, 3, 2, 1, 38, 55, 78, DateTimeKind.Local).AddTicks(5791));
        }
    }
}
