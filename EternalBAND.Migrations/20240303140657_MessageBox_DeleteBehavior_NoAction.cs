using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EternalBAND.Migrations
{
    /// <inheritdoc />
    public partial class MessageBox_DeleteBehavior_NoAction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "AddedDate",
                value: new DateTime(2024, 3, 3, 0, 16, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "AddedDate",
                value: new DateTime(2024, 3, 3, 0, 16, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "AddedDate",
                value: new DateTime(2024, 3, 3, 0, 16, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "AddedDate",
                value: new DateTime(2024, 3, 3, 0, 2, 36, 211, DateTimeKind.Local).AddTicks(2799));

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "AddedDate",
                value: new DateTime(2024, 3, 3, 0, 2, 36, 211, DateTimeKind.Local).AddTicks(2971));

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "AddedDate",
                value: new DateTime(2024, 3, 3, 0, 2, 36, 211, DateTimeKind.Local).AddTicks(2975));
        }
    }
}
