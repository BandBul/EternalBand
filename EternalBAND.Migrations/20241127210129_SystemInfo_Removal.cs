using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EternalBAND.Migrations
{
    /// <inheritdoc />
    public partial class SystemInfo_Removal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemInfo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SystemInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemInfo", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "SystemInfo",
                columns: new[] { "Id", "Desc", "Type", "Value" },
                values: new object[,]
                {
                    { 1, "Mail Gönderen Adres", "mail-sender-address", "testuservortex@outlook.com" },
                    { 2, "Mail Gönderen Sunucu Portu", "mail-sender-port", "587" },
                    { 3, "Mail Gönderen Sunucu Hostu", "mail-sender-host", "smtp-mail.outlook.com" },
                    { 4, "Mail Gönderen Sunucu SSL İstiyor mu?", "mail-sender-enable-ssl", "true" },
                    { 5, "Mail Gönderen Adres Şifresi", "mail-sender-address-password", "K48F7HWiFk7Abgjn" },
                    { 6, "Site Başlığı", "site-title", "EternalBAND" },
                    { 7, "Site Facebook Adres (Boş bırakırsanız göstermez)", "site-facebook", "https://facebook.com" },
                    { 8, "Site İletişim Mail Adresi (Boş bırakırsanız göstermez)", "site-mail-address", "test@mail.com" },
                    { 9, "Site Instagram Adres (Boş bırakırsanız göstermez)", "site-instagram", "https://instagram.com" },
                    { 10, "Site Facebook Adres (Boş bırakırsanız göstermez)", "site-twitter", "https://twitter.com" },
                    { 11, "Site Telefon Numarası (Boş bırakırsanız göstermez)", "site-phone", "0850" },
                    { 12, "Adres (Boş bırakırsanız göstermez)", "site-address", "Istanbul" },
                    { 16, "Site Bottom Footer Text", "site-bottom-footer-text", "- Tüm hakları saklıdır." },
                    { 17, "Site Sol Alt Metin", "site-footer-left-text", " It is a long established fact that a reader will be of a page reader will be of at its layout. " },
                    { 18, "Site Desc Seo için", "site-desc", "Buraya description gelecek :)" },
                    { 19, "Site Keywords Seo için", "site-keywords", "keywords , keywords 1, keywords 2" }
                });
        }
    }
}
