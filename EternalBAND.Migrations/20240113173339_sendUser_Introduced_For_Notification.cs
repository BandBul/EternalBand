using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EternalBAND.Data.Migrations
{
    /// <inheritdoc />
    public partial class sebdeUSer_Introduced_For_Notification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SenderUserId",
                table: "Notification",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "AddedDate",
                value: new DateTime(2024, 1, 13, 18, 33, 39, 437, DateTimeKind.Local).AddTicks(246));

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "AddedDate",
                value: new DateTime(2024, 1, 13, 18, 33, 39, 437, DateTimeKind.Local).AddTicks(303));

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "AddedDate",
                value: new DateTime(2024, 1, 13, 18, 33, 39, 437, DateTimeKind.Local).AddTicks(306));

            migrationBuilder.CreateIndex(
                name: "IX_Notification_SenderUserId",
                table: "Notification",
                column: "SenderUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_AspNetUsers_SenderUserId",
                table: "Notification",
                column: "SenderUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_AspNetUsers_SenderUserId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_SenderUserId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "SenderUserId",
                table: "Notification");

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "AddedDate",
                value: new DateTime(2024, 1, 6, 14, 41, 33, 977, DateTimeKind.Local).AddTicks(8212));

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "AddedDate",
                value: new DateTime(2024, 1, 6, 14, 41, 33, 977, DateTimeKind.Local).AddTicks(8268));

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "AddedDate",
                value: new DateTime(2024, 1, 6, 14, 41, 33, 977, DateTimeKind.Local).AddTicks(8320));
        }
    }
}
