using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewPortfolio.Migrations
{
    public partial class asuzjf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameUser",
                table: "Article",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameUser",
                table: "Article");
        }
    }
}
