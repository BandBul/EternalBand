using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EternalBAND.Migrations
{
    /// <inheritdoc />
    public partial class postId_introduce_for_messages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RelatedPostId",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "AddedDate",
                value: new DateTime(2023, 12, 17, 15, 58, 13, 1, DateTimeKind.Local).AddTicks(9832));

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "AddedDate",
                value: new DateTime(2023, 12, 17, 15, 58, 13, 1, DateTimeKind.Local).AddTicks(9881));

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "AddedDate",
                value: new DateTime(2023, 12, 17, 15, 58, 13, 1, DateTimeKind.Local).AddTicks(9883));

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RelatedPostId",
                table: "Messages",
                column: "RelatedPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Posts_RelatedPostId",
                table: "Messages",
                column: "RelatedPostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Posts_RelatedPostId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_RelatedPostId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "RelatedPostId",
                table: "Messages");

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "AddedDate",
                value: new DateTime(2023, 9, 14, 23, 33, 16, 431, DateTimeKind.Local).AddTicks(4389));

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "AddedDate",
                value: new DateTime(2023, 9, 14, 23, 33, 16, 431, DateTimeKind.Local).AddTicks(4440));

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "AddedDate",
                value: new DateTime(2023, 9, 14, 23, 33, 16, 431, DateTimeKind.Local).AddTicks(4444));
        }
    }
}
