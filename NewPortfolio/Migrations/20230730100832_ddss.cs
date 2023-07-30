using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewPortfolio.Migrations
{
    public partial class ddss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountPost",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "Credits",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "DateOfRegister",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "NickName",
                table: "Article");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountPost",
                table: "Article",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Credits",
                table: "Article",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DateOfRegister",
                table: "Article",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Article",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NickName",
                table: "Article",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
