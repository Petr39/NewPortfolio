using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewPortfolio.Migrations
{
    public partial class GameINItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_GameId",
                table: "Items",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Games_GameId",
                table: "Items",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Games_GameId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_GameId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Items");
        }
    }
}
