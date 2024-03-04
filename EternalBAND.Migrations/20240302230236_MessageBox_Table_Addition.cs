using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EternalBAND.Migrations
{
    /// <inheritdoc />
    public partial class MessageBox_Table_Addition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MessageBoxId",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MessageBoxes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Recipient1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Recipient2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: true),
                    PostTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPostDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageBoxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageBoxes_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Messages_MessageBoxId",
                table: "Messages",
                column: "MessageBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageBoxes_PostId",
                table: "MessageBoxes",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_MessageBoxes_MessageBoxId",
                table: "Messages",
                column: "MessageBoxId",
                principalTable: "MessageBoxes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_MessageBoxes_MessageBoxId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "MessageBoxes");

            migrationBuilder.DropIndex(
                name: "IX_Messages_MessageBoxId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "MessageBoxId",
                table: "Messages");

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "AddedDate",
                value: new DateTime(2024, 3, 2, 16, 9, 8, 526, DateTimeKind.Local).AddTicks(5186));

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "AddedDate",
                value: new DateTime(2024, 3, 2, 16, 9, 8, 526, DateTimeKind.Local).AddTicks(5248));

            migrationBuilder.UpdateData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "AddedDate",
                value: new DateTime(2024, 3, 2, 16, 9, 8, 526, DateTimeKind.Local).AddTicks(5252));
        }
    }
}
