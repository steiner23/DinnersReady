using Microsoft.EntityFrameworkCore.Migrations;

namespace DinnersReady.Migrations
{
    public partial class AddIngredInstructIdents2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipeIdent",
                table: "RecipeInstructions");

            migrationBuilder.DropColumn(
                name: "RecipeIdent",
                table: "RecipeIngredients");

            migrationBuilder.AddColumn<string>(
                name: "RecipeInstructionIdent",
                table: "RecipeInstructions",
                maxLength: 16,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecipeIngredientIdent",
                table: "RecipeIngredients",
                maxLength: 16,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipeInstructionIdent",
                table: "RecipeInstructions");

            migrationBuilder.DropColumn(
                name: "RecipeIngredientIdent",
                table: "RecipeIngredients");

            migrationBuilder.AddColumn<string>(
                name: "RecipeIdent",
                table: "RecipeInstructions",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecipeIdent",
                table: "RecipeIngredients",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");
        }
    }
}
