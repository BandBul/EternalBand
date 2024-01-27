using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EternalBAND.Migrations
{
    /// <inheritdoc />
    public partial class FK_Messages_Posts_RelatedPostId_Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Posts_RelatedPostId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Instruments_InstrumentsId",
                table: "Posts");

            migrationBuilder.AlterColumn<int>(
                name: "InstrumentsId",
                table: "Posts",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Instruments_InstrumentsId",
                table: "Posts",
                column: "InstrumentsId",
                principalTable: "Instruments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Posts_RelatedPostId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Instruments_InstrumentsId",
                table: "Posts");

            migrationBuilder.AlterColumn<int>(
                name: "InstrumentsId",
                table: "Posts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Posts_RelatedPostId",
                table: "Messages",
                column: "RelatedPostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Instruments_InstrumentsId",
                table: "Posts",
                column: "InstrumentsId",
                principalTable: "Instruments",
                principalColumn: "Id");
        }
    }
}
