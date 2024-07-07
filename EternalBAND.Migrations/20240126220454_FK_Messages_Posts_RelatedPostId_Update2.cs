using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EternalBAND.Migrations
{
    /// <inheritdoc />
    public partial class FK_Messages_Posts_RelatedPostId_Update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Posts_RelatedPostId",
                table: "Messages");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Posts_RelatedPostId",
                table: "Messages");

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "AddedDate",
                value: new DateTime(2024, 1, 26, 22, 22, 18, 156, DateTimeKind.Local).AddTicks(3930));

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "AddedDate",
                value: new DateTime(2024, 1, 26, 22, 22, 18, 156, DateTimeKind.Local).AddTicks(3978));

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "AddedDate",
                value: new DateTime(2024, 1, 26, 22, 22, 18, 156, DateTimeKind.Local).AddTicks(3981));

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Posts_RelatedPostId",
                table: "Messages",
                column: "RelatedPostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}
