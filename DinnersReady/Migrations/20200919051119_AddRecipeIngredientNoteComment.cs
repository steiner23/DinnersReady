using Microsoft.EntityFrameworkCore.Migrations;

namespace DinnersReady.Migrations
{
    public partial class AddRecipeIngredientNoteComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "RecipeIngredients",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "RecipeIngredients");
        }
    }
}
