using Microsoft.EntityFrameworkCore.Migrations;

namespace DinnersReady.Migrations
{
    public partial class AddIngredInstructIdents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecipeIdent",
                table: "RecipeInstructions",
                maxLength: 16,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecipeIdent",
                table: "RecipeIngredients",
                maxLength: 16,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipeIdent",
                table: "RecipeInstructions");

            migrationBuilder.DropColumn(
                name: "RecipeIdent",
                table: "RecipeIngredients");
        }
    }
}
