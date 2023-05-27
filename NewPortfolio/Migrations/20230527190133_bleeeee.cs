using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewPortfolio.Migrations
{
    public partial class bleeeee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Credit",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Credit",
                table: "AspNetUsers");
        }
    }
}
