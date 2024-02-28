using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EternalBAND.Migrations
{
    /// <inheritdoc />
    public partial class Add_TargetGroup_For_PostsType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TargetGroup",
                table: "PostTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TargetGroupName",
                table: "PostTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "HtmlText",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AddedDate", "TargetGroup", "TargetGroupName" },
                values: new object[] { new DateTime(2024, 2, 28, 0, 30, 1, 660, DateTimeKind.Local).AddTicks(8016), 2, "Grup Arıyorum" });

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AddedDate", "TargetGroup", "TargetGroupName" },
                values: new object[] { new DateTime(2024, 2, 28, 0, 30, 1, 660, DateTimeKind.Local).AddTicks(8071), 1, "Müzisyen Arıyorum" });

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AddedDate", "TargetGroup", "TargetGroupName" },
                values: new object[] { new DateTime(2024, 2, 28, 0, 30, 1, 660, DateTimeKind.Local).AddTicks(8075), 3, "Ders Almak İstiyorum" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetGroup",
                table: "PostTypes");

            migrationBuilder.DropColumn(
                name: "TargetGroupName",
                table: "PostTypes");

            migrationBuilder.AlterColumn<string>(
                name: "HtmlText",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "AddedDate",
                value: new DateTime(2024, 1, 27, 14, 51, 45, 699, DateTimeKind.Local).AddTicks(7428));

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "AddedDate",
                value: new DateTime(2024, 1, 27, 14, 51, 45, 699, DateTimeKind.Local).AddTicks(7482));

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "AddedDate",
                value: new DateTime(2024, 1, 27, 14, 51, 45, 699, DateTimeKind.Local).AddTicks(7485));
        }
    }
}
