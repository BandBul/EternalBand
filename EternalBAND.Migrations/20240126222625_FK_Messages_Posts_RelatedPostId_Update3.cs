using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EternalBAND.Migrations
{
    /// <inheritdoc />
    public partial class FK_Messages_Posts_RelatedPostId_Update3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Posts_RelatedPostId",
                table: "Messages");

            migrationBuilder.AlterColumn<int>(
                name: "RelatedPostId",
                table: "Messages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "AddedDate",
                value: new DateTime(2024, 1, 26, 23, 26, 24, 716, DateTimeKind.Local).AddTicks(7908));

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "AddedDate",
                value: new DateTime(2024, 1, 26, 23, 26, 24, 716, DateTimeKind.Local).AddTicks(7957));

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "AddedDate",
                value: new DateTime(2024, 1, 26, 23, 26, 24, 716, DateTimeKind.Local).AddTicks(7960));

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Posts_RelatedPostId",
                table: "Messages",
                column: "RelatedPostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Posts_RelatedPostId",
                table: "Messages");

            migrationBuilder.AlterColumn<int>(
                name: "RelatedPostId",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "AddedDate",
                value: new DateTime(2024, 1, 26, 23, 4, 54, 180, DateTimeKind.Local).AddTicks(599));

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "AddedDate",
                value: new DateTime(2024, 1, 26, 23, 4, 54, 180, DateTimeKind.Local).AddTicks(645));

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "AddedDate",
                value: new DateTime(2024, 1, 26, 23, 4, 54, 180, DateTimeKind.Local).AddTicks(648));

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Posts_RelatedPostId",
                table: "Messages",
                column: "RelatedPostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
