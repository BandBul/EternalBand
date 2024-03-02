using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EternalBAND.Migrations
{
    /// <inheritdoc />
    public partial class Changing_PostType_Columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeShort",
                table: "PostTypes");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "PostTypes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "SearchName",
                table: "PostTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AddedDate", "SearchName", "Type" },
                values: new object[] { new DateTime(2024, 3, 1, 0, 56, 56, 466, DateTimeKind.Local).AddTicks(3820), "Müzisyen Arıyorum", "Musician" });

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AddedDate", "SearchName", "Type" },
                values: new object[] { new DateTime(2024, 3, 1, 0, 56, 56, 466, DateTimeKind.Local).AddTicks(3869), "Grup Arıyorum", "Group" });

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AddedDate", "SearchName", "Type" },
                values: new object[] { new DateTime(2024, 3, 1, 0, 56, 56, 466, DateTimeKind.Local).AddTicks(3874), "Ders Vermek İstiyorum", "Lesson" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SearchName",
                table: "PostTypes");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "PostTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeShort",
                table: "PostTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AddedDate", "Type", "TypeShort" },
                values: new object[] { new DateTime(2024, 2, 28, 0, 30, 1, 660, DateTimeKind.Local).AddTicks(8016), "Müzisyen Arıyorum", "Musician" });

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AddedDate", "Type", "TypeShort" },
                values: new object[] { new DateTime(2024, 2, 28, 0, 30, 1, 660, DateTimeKind.Local).AddTicks(8071), "Grup Arıyorum", "Group" });

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AddedDate", "Type", "TypeShort" },
                values: new object[] { new DateTime(2024, 2, 28, 0, 30, 1, 660, DateTimeKind.Local).AddTicks(8075), "Ders Vermek İstiyorum", "Lesson" });
        }
    }
}
