using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EternalBAND.Migrations
{
    /// <inheritdoc />
    public partial class Instruments_And_PostTypes_Initilization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Instruments",
                columns: new[] { "Id", "Instrument", "InstrumentShort", "IsActive" },
                values: new object[,]
                {
                    { 1, "Gitar", "Guitar", true },
                    { 2, "Bas Gitar", "Bass Guitar", true },
                    { 3, "Davul", "Drum", true },
                    { 4, "Piano", "Piano", true },
                    { 5, "Klavye", "Keyboard", true },
                    { 6, "Saksafon", "Saxophone", true },
                    { 7, "Keman", "Violin", true },
                    { 8, "Vokal", "Vocal", true },
                    { 9, "Kontrabas", "Kontrabas", true }
                });

            migrationBuilder.InsertData(
                table: "PostTypes",
                columns: new[] { "Id", "Active", "AddedDate", "Type", "TypeShort" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2023, 9, 14, 10, 24, 4, 784, DateTimeKind.Local).AddTicks(2371), "Müzisyen Arıyorum", "Musician" },
                    { 2, true, new DateTime(2023, 9, 14, 10, 24, 4, 784, DateTimeKind.Local).AddTicks(2419), "Grup Arıyorum", "Group" },
                    { 3, true, new DateTime(2023, 9, 14, 10, 24, 4, 784, DateTimeKind.Local).AddTicks(2421), "Ders Vermek İstiyorum", "Lesson" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Instruments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Instruments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Instruments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Instruments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Instruments",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Instruments",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Instruments",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Instruments",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Instruments",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PostTypes",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
