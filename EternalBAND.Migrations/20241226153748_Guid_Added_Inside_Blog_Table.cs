using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EternalBAND.Migrations
{
    /// <inheritdoc />
    public partial class Guid_Added_Inside_Blog_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "Blogs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "13362109-dc20-49ad-8b05-db533707a22f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6816aac2-3448-47e9-bdbc-54c8a347c8d4", "AQAAAAIAAYagAAAAEGSxNCu4wMt6kEDKflsB4iDuKt2yS7sT6hJkDPGcErKguUVkZfUrVAfG9k5fJ0UmWg==", "ce732604-2c07-4228-825d-6e49abb26ef3" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93816595-56df-4287-b5f1-1a9d36871222",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AQAAAAIAAYagAAAAEGO/WArxg5ovBMlRGIQnnEEfmuvZHhPq2g7hS2WSnZzD1kIUxYCremYES+B1wY4uHg==", "ce732604-2c07-4228-825d-6e49abb26ef3" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d15a305f-ca73-4417-9143-4c3c0628e88d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6816aac2-3448-47e9-bdbc-54c8a347c8d4", "AQAAAAIAAYagAAAAEK+AcmaaI8WodUj1b5Xy1b5+hqiuMgKZZ3p9wYV9zLYYgH5a1iFjTtDrLEItiOEbXg==", "ce732604-2c07-4228-825d-6e49abb26ef3" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Blogs");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "13362109-dc20-49ad-8b05-db533707a22f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ce732604-2c07-4228-825d-6e49abb26ef3", "AQAAAAIAAYagAAAAEI1kLoa4DMlF9Alk6fu0DpWDH8TU15YGQA4YNvSsXIkL0XbIBRJEHlAjIr0p9yjidQ==", "00596ee8-f36a-4f35-b215-d0372d675e7f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93816595-56df-4287-b5f1-1a9d36871222",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AQAAAAIAAYagAAAAEAllLFY7eIzolNTfUPMPcRP6vjr8eN1dkLeueS3EWNQfSZYwwnmVFLYVGjr7G/3UTg==", "56e09ffe-bec4-4535-8dce-bbba31d36958" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d15a305f-ca73-4417-9143-4c3c0628e88d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4bf84a4d-e00b-40d8-abe5-46f868502c25", "AQAAAAIAAYagAAAAECuofwzUWJxJymaAaCxvb4UKEadpxWYTuZElbuLTiJL0JAVvg8LuuErjEZxDXIza6A==", "91218c32-e1ff-43cf-ab68-7c820039f995" });
        }
    }
}
