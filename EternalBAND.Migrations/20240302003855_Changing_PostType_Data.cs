using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EternalBAND.Migrations
{
    /// <inheritdoc />
    public partial class Changing_PostType_Data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetGroupName",
                table: "PostTypes");

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AddedDate", "SearchName" },
                values: new object[] { new DateTime(2024, 3, 2, 1, 38, 55, 78, DateTimeKind.Local).AddTicks(5732), "Grup Arıyorum" });

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AddedDate", "SearchName" },
                values: new object[] { new DateTime(2024, 3, 2, 1, 38, 55, 78, DateTimeKind.Local).AddTicks(5787), "Müzisyen Arıyorum" });

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AddedDate", "SearchName" },
                values: new object[] { new DateTime(2024, 3, 2, 1, 38, 55, 78, DateTimeKind.Local).AddTicks(5791), "Ders Almak İstiyorum" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TargetGroupName",
                table: "PostTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AddedDate", "SearchName", "TargetGroupName" },
                values: new object[] { new DateTime(2024, 3, 1, 0, 56, 56, 466, DateTimeKind.Local).AddTicks(3820), "Müzisyen Arıyorum", "Grup Arıyorum" });

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AddedDate", "SearchName", "TargetGroupName" },
                values: new object[] { new DateTime(2024, 3, 1, 0, 56, 56, 466, DateTimeKind.Local).AddTicks(3869), "Grup Arıyorum", "Müzisyen Arıyorum" });

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AddedDate", "SearchName", "TargetGroupName" },
                values: new object[] { new DateTime(2024, 3, 1, 0, 56, 56, 466, DateTimeKind.Local).AddTicks(3874), "Ders Vermek İstiyorum", "Ders Almak İstiyorum" });
        }
    }
}
