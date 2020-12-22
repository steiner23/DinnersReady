using System.Collections.Generic;
using DinnersReadyCore.Models.Recipes;


namespace DinnersReadyCore.ViewModels.ViewRecipe
{
    public class ViewVM
    {
        public RecipeWithDetails localRecipe { get; set; }

        public List<string> MainFoodCategories { get; set; }

        public List<string> IngredientsLike { get; set; }

        public List<string> UnitsOfMeasureSymbols { get; set; }

    }
}


