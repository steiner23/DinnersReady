using System.Collections.Generic;


namespace DinnersReadyCore.ViewModels.AddRecipe
{
    public class AddVM
    {

        public string Name { get; set; }

        public bool RecipeAddedSuccess { get; set; }

        public List<string> MainFoodCategories { get; set; }

        public List<string> IngredientsLike { get; set; }

        public List<string> UnitsOfMeasureSymbols { get; set; }

    }
}


