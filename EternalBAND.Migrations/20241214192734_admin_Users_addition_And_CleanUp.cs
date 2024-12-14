using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EternalBAND.Migrations
{
    /// <inheritdoc />
    public partial class admin_Users_addition_And_CleanUp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "248feb45-0294-450c-afd2-9c6de89023b9");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "ErrorLogs");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "39d321fc-0911-4412-a19e-98fb7d068440", "113819d4-128b-41c1-9676-1cccb59ddf87" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "39d321fc-0911-4412-a19e-98fb7d068440", "a8b78fa6-7f18-4daa-8181-f336d6dafa5e" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "39d321fc-0911-4412-a19e-98fb7d068440", "ffb8befe-c8da-43d7-b592-92d9671ce99f" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "113819d4-128b-41c1-9676-1cccb59ddf87");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a8b78fa6-7f18-4daa-8181-f336d6dafa5e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ffb8befe-c8da-43d7-b592-92d9671ce99f");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Age", "City", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PhotoPath", "RegistrationDate", "SecurityStamp", "Surname", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "13362109-dc20-49ad-8b05-db533707a22f", 0, 0, 0, "ce732604-2c07-4228-825d-6e49abb26ef3", "admin.engin@bandbul.com", true, "Admin", true, null, "Admin", "ADMIN.ENGIN@BANDBUL.COM", "ADMIN.ENGIN@BANDBUL.COM", "AQAAAAIAAYagAAAAEI1kLoa4DMlF9Alk6fu0DpWDH8TU15YGQA4YNvSsXIkL0XbIBRJEHlAjIr0p9yjidQ==", "5000000000", false, "/images/user_photo/profile.png", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "00596ee8-f36a-4f35-b215-d0372d675e7f", "", false, "admin.engin@bandbul.com" },
                    { "93816595-56df-4287-b5f1-1a9d36871222", 0, 0, 0, "6816aac2-3448-47e9-bdbc-54c8a347c8d4", "admin.enis@bandbul.com", true, "Admin", true, null, "Admin", "ADMIN.ENIS@BANDBUL.COM", "ADMIN.ENIS@BANDBUL.COM", "AQAAAAIAAYagAAAAEAllLFY7eIzolNTfUPMPcRP6vjr8eN1dkLeueS3EWNQfSZYwwnmVFLYVGjr7G/3UTg==", "5000000000", false, "/images/user_photo/profile.png", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "56e09ffe-bec4-4535-8dce-bbba31d36958", "", false, "admin.enis@bandbul.com" },
                    { "d15a305f-ca73-4417-9143-4c3c0628e88d", 0, 0, 0, "4bf84a4d-e00b-40d8-abe5-46f868502c25", "admin.berkay@bandbul.com", true, "Admin", true, null, "Admin", "ADMIN.BERKAY@BANDBUL.COM", "ADMIN.BERKAY@BANDBUL.COM", "AQAAAAIAAYagAAAAECuofwzUWJxJymaAaCxvb4UKEadpxWYTuZElbuLTiJL0JAVvg8LuuErjEZxDXIza6A==", "5000000000", false, "/images/user_photo/profile.png", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "91218c32-e1ff-43cf-ab68-7c820039f995", "", false, "admin.berkay@bandbul.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "39d321fc-0911-4412-a19e-98fb7d068440", "13362109-dc20-49ad-8b05-db533707a22f" },
                    { "39d321fc-0911-4412-a19e-98fb7d068440", "93816595-56df-4287-b5f1-1a9d36871222" },
                    { "39d321fc-0911-4412-a19e-98fb7d068440", "d15a305f-ca73-4417-9143-4c3c0628e88d" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "39d321fc-0911-4412-a19e-98fb7d068440", "13362109-dc20-49ad-8b05-db533707a22f" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "39d321fc-0911-4412-a19e-98fb7d068440", "93816595-56df-4287-b5f1-1a9d36871222" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "39d321fc-0911-4412-a19e-98fb7d068440", "d15a305f-ca73-4417-9143-4c3c0628e88d" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "13362109-dc20-49ad-8b05-db533707a22f");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "93816595-56df-4287-b5f1-1a9d36871222");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d15a305f-ca73-4417-9143-4c3c0628e88d");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Age", "City", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PhotoPath", "RegistrationDate", "SecurityStamp", "Surname", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "113819d4-128b-41c1-9676-1cccb59ddf87", 0, 0, 0, "f5d1d90c-0bfc-4b88-876d-813096977b06", "admin.engin@bandbul.com", true, "admin.engin@bandbul.com Admin", true, null, "admin.engin@bandbul.com", "ADMIN.ENGIN@BANDBUL.COM", "ADMIN.ENGIN@BANDBUL.COM", "AQAAAAIAAYagAAAAEA6aOmvUs2xfV/dxRp7gNprQxFAKNmnvQei9OX3gHpgNw51Mwf6KkqfVRaCpDm5+5A==", "5000000000", false, "/images/user_photo/profile.png", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1223f558-b0f4-47b0-80bd-ab4d5fe3536e", "Admin", false, "admin.engin@bandbul.com" },
                    { "a8b78fa6-7f18-4daa-8181-f336d6dafa5e", 0, 0, 0, "5b177467-2be6-42cd-899f-738cddc7e3a8", "admin.berkay@bandbul.com", true, "admin.berkay@bandbul.com Admin", true, null, "admin.berkay@bandbul.com", "ADMIN.BERKAY@BANDBUL.COM", "ADMIN.BERKAY@BANDBUL.COM", "AQAAAAIAAYagAAAAEJOOr3KQiOT9IIs1W6aER7sTvF34w+YKfAyfgq5wq8E6yCRW/eaZ4H2X/a4vTobpGQ==", "5000000000", false, "/images/user_photo/profile.png", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a9682973-e2bf-429b-9a56-bd6c2fc1678f", "Admin", false, "admin.berkay@bandbul.com" },
                    { "ffb8befe-c8da-43d7-b592-92d9671ce99f", 0, 0, 0, "6fab56c6-18c9-404c-96ae-7a88d28701e4", "admin.enis@bandbul.com", true, "admin.enis@bandbul.com Admin", true, null, "admin.enis@bandbul.com", "ADMIN.ENIS@BANDBUL.COM", "ADMIN.ENIS@BANDBUL.COM", "AQAAAAIAAYagAAAAEI1fEkS1eut5d4814Bn/69bqF4/jVgWKQGUjRkjYGmzJ26RvRYWfgRr61R7ruK8E9Q==", "5000000000", false, "/images/user_photo/profile.png", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "efb2d082-ba0b-4348-932e-8b1d6dead98b", "Admin", false, "admin.enis@bandbul.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "39d321fc-0911-4412-a19e-98fb7d068440", "113819d4-128b-41c1-9676-1cccb59ddf87" },
                    { "39d321fc-0911-4412-a19e-98fb7d068440", "a8b78fa6-7f18-4daa-8181-f336d6dafa5e" },
                    { "39d321fc-0911-4412-a19e-98fb7d068440", "ffb8befe-c8da-43d7-b592-92d9671ce99f" }
                });
        }
    }
}
