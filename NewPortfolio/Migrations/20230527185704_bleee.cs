using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewPortfolio.Migrations
{
    public partial class bleee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NameUser",
                table: "Article",
                newName: "NickName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NickName",
                table: "Article",
                newName: "NameUser");
        }
    }
}
