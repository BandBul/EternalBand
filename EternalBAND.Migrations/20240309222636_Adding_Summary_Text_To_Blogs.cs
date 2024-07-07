using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EternalBAND.Migrations
{
    /// <inheritdoc />
    public partial class Adding_Summary_Text_To_Blogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SummaryText",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SummaryText",
                table: "Blogs");
        }
    }
}
