using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DinnersReady.Migrations
{
    public partial class DatabaseInitialRecipeNV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FoodCategories",
                columns: table => new
                {
                    FoodCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FoodCategoryName = table.Column<string>(maxLength: 50, nullable: false),
                    DerivedFoodCategoryName = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodCategories", x => x.FoodCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "RecipeImages",
                columns: table => new
                {
                    ImageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageName = table.Column<string>(maxLength: 100, nullable: false),
                    ContentType = table.Column<string>(maxLength: 200, nullable: false),
                    Image = table.Column<byte[]>(nullable: false),
                    RecipeIdent = table.Column<string>(maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeImages", x => x.ImageId);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    RecipeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 150, nullable: false),
                    Description = table.Column<string>(maxLength: 300, nullable: true),
                    Course = table.Column<string>(maxLength: 30, nullable: false),
                    CookMethod = table.Column<string>(maxLength: 30, nullable: false),
                    Servings = table.Column<int>(nullable: false),
                    CookingTime = table.Column<string>(maxLength: 30, nullable: false),
                    GenDate = table.Column<DateTime>(nullable: false),
                    RecipeIdent = table.Column<string>(maxLength: 16, nullable: false),
                    AdditionalNotes = table.Column<string>(maxLength: 1000, nullable: true),
                    GoesWith = table.Column<string>(maxLength: 150, nullable: true),
                    ViewCount = table.Column<int>(nullable: true),
                    UserLikes = table.Column<int>(nullable: true),
                    Season = table.Column<string>(maxLength: 30, nullable: true),
                    Cuisine = table.Column<string>(maxLength: 30, nullable: true),
                    Event = table.Column<string>(maxLength: 30, nullable: true),
                    DietaryRequirement = table.Column<string>(maxLength: 30, nullable: true),
                    Freezable = table.Column<bool>(nullable: true),
                    ReviewedStatus = table.Column<bool>(nullable: true),
                    MainFoodCategory = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.RecipeId);
                });

            migrationBuilder.CreateTable(
                name: "UnitsOfMeasures",
                columns: table => new
                {
                    UnitOfMeasureId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitOfMeasureSymbol = table.Column<string>(maxLength: 10, nullable: false),
                    UnitOfMeasureName = table.Column<string>(maxLength: 20, nullable: false),
                    UnitOfMeasureNamePlural = table.Column<string>(maxLength: 20, nullable: false),
                    BaseUnitOfMeasureId = table.Column<int>(nullable: false),
                    MultiplyConversionToBaseRate = table.Column<double>(nullable: false),
                    MultiplyConversionToIngredientRate = table.Column<double>(nullable: false),
                    IsBaseUnitOfMeasure = table.Column<bool>(nullable: false),
                    SystemOfMeasureName = table.Column<string>(maxLength: 30, nullable: false),
                    DryOrWetUnitOfMeasure = table.Column<string>(maxLength: 15, nullable: false),
                    DisplayUomName = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitsOfMeasures", x => x.UnitOfMeasureId);
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    IngredientId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameSingular = table.Column<string>(maxLength: 50, nullable: false),
                    NamePlural = table.Column<string>(maxLength: 50, nullable: false),
                    FoodCategoryId = table.Column<int>(nullable: false),
                    TypeOfMeasureName = table.Column<string>(maxLength: 15, nullable: false),
                    ReviewedStatus = table.Column<bool>(nullable: true),
                    OneGramToTeaspoonUK = table.Column<double>(nullable: false),
                    OneTeaspoonUKToGram = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.IngredientId);
                    table.ForeignKey(
                        name: "FK_Ingredients_FoodCategories_FoodCategoryId",
                        column: x => x.FoodCategoryId,
                        principalTable: "FoodCategories",
                        principalColumn: "FoodCategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeInstructions",
                columns: table => new
                {
                    InstructionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<int>(nullable: false),
                    InstructionStepId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: false),
                    TemperatureValue = table.Column<double>(nullable: false),
                    OriginalTemperatureUnitOfMeasure = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeInstructions", x => x.InstructionId);
                    table.ForeignKey(
                        name: "FK_RecipeInstructions_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeIngredients",
                columns: table => new
                {
                    RecipeIngredientId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<int>(nullable: false),
                    IngredientId = table.Column<int>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    OriginalUnitOfMeasure = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeIngredients", x => x.RecipeIngredientId);
                    table.ForeignKey(
                        name: "FK_RecipeIngredients_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "IngredientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeIngredients_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_FoodCategoryId",
                table: "Ingredients",
                column: "FoodCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_IngredientId",
                table: "RecipeIngredients",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_RecipeId",
                table: "RecipeIngredients",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeInstructions_RecipeId",
                table: "RecipeInstructions",
                column: "RecipeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeImages");

            migrationBuilder.DropTable(
                name: "RecipeIngredients");

            migrationBuilder.DropTable(
                name: "RecipeInstructions");

            migrationBuilder.DropTable(
                name: "UnitsOfMeasures");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "FoodCategories");
        }
    }
}
