using DinnersReadyCore.Models.FoodCategories;
using DinnersReadyCore.Models.Ingredients;
using DinnersReadyCore.Models.RecipeInstructions;
using DinnersReadyCore.Models.RecipeIngredients;
using DinnersReadyCore.Models.RecipeImages;
using DinnersReadyCore.Models.UnitsOfMeasures;
using Microsoft.EntityFrameworkCore;



namespace DinnersReadyCore.Models
{

    public class RecipeContext : DbContext
    {

    //Future Use: public class RecipeContext : IdentityDbContext<ApplicationUser>

        public RecipeContext(DbContextOptions<RecipeContext> options) : base(options)
        { }
        
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<FoodCategory> FoodCategories { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RecipeInstruction> RecipeInstructions { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<RecipeImage> RecipeImages { get; set; }       
        public DbSet<UnitOfMeasure> UnitsOfMeasures { get; set; }

    }
}