using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EternalBAND.Migrations
{
    /// <inheritdoc />
    public partial class Changing_PostType_Data3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetGroup",
                table: "PostTypes");

            migrationBuilder.RenameColumn(
                name: "SearchName",
                table: "PostTypes",
                newName: "PostCreateText");

            migrationBuilder.AddColumn<string>(
                name: "FilterText",
                table: "PostTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AddedDate", "FilterText", "Type", "TypeText" },
                values: new object[] { new DateTime(2024, 3, 2, 16, 9, 8, 526, DateTimeKind.Local).AddTicks(5186), "Grup Arıyorum", "Group", "Grup İlanları" });

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AddedDate", "FilterText", "Type", "TypeText" },
                values: new object[] { new DateTime(2024, 3, 2, 16, 9, 8, 526, DateTimeKind.Local).AddTicks(5248), "Müzisyen Arıyorum", "Musician", "Müzisyen İlanları" });

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AddedDate", "FilterText", "PostCreateText" },
                values: new object[] { new DateTime(2024, 3, 2, 16, 9, 8, 526, DateTimeKind.Local).AddTicks(5252), "Ders Almak İstiyorum", "Ders Vermek İstiyorum" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilterText",
                table: "PostTypes");

            migrationBuilder.RenameColumn(
                name: "PostCreateText",
                table: "PostTypes",
                newName: "SearchName");

            migrationBuilder.AddColumn<int>(
                name: "TargetGroup",
                table: "PostTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AddedDate", "TargetGroup", "Type", "TypeText" },
                values: new object[] { new DateTime(2024, 3, 2, 2, 34, 43, 806, DateTimeKind.Local).AddTicks(82), 2, "Musician", "Müzisyen İlanları" });

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AddedDate", "TargetGroup", "Type", "TypeText" },
                values: new object[] { new DateTime(2024, 3, 2, 2, 34, 43, 806, DateTimeKind.Local).AddTicks(134), 1, "Group", "Grup İlanları" });

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AddedDate", "SearchName", "TargetGroup" },
                values: new object[] { new DateTime(2024, 3, 2, 2, 34, 43, 806, DateTimeKind.Local).AddTicks(138), "Ders Almak İstiyorum", 3 });
        }
    }
}
