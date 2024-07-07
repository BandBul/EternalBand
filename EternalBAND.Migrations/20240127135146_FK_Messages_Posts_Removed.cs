using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EternalBAND.Migrations
{
    /// <inheritdoc />
    public partial class FK_Messages_Posts_Removed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Posts_RelatedPostId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_RelatedPostId",
                table: "Messages");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                onDelete: ReferentialAction.SetNull);
        }
    }
}
