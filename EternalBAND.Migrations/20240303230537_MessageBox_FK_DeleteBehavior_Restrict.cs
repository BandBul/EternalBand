﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EternalBAND.Migrations
{
    /// <inheritdoc />
    public partial class MessageBox_FK_DeleteBehavior_Restrict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageBoxes_Posts_PostId",
                table: "MessageBoxes");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageBoxes_Posts_PostId",
                table: "MessageBoxes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageBoxes_Posts_PostId",
                table: "MessageBoxes");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageBoxes_Posts_PostId",
                table: "MessageBoxes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}
