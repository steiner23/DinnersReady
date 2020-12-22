using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;


namespace DinnersReadyCore.Models.Recipes
{
    public interface IRecipeRepository
    {

        public List<string> GetRecipesLike(string userInput, int numberOfRecipes);

        public List<string> GetIngredientsLike(string userInput, int numberOfIngredients);

        public IEnumerable<Recipe> GetRecipes(string orderBy, bool ascending, int numberOfRecipes);

        public IQueryable<Recipe> GetARecipe(string recipeName);

        public IQueryable<Recipe> GetARecipe(int recipeId);

        public int GetARecipeByGenData(string genDate);

        public RecipeWithDetails GetRecipeWithDetails(string recipeIdent);

        public string ProcessRecipe(string recipeName, string description, 
            string course, string cookingMethod, string servings, string cookingTime, 
            string additionalNotes, string goesWithNotes, string season, string cuisine, string anevent,
            string dietaryRequirement, bool freezable, string mainFoodCategory,
            List<string>  ingredientsName, List<string> amountsName, List<string> uomName,
            List<string> commentsName, List<string> instructionsName, List<string> instructionTempName,
            List<string> originalTemperatureUoMName, string recipeIdt);

        public string AddRecipe(string recipeName, string description, 
            string course, string cookingMethod, string servings, string cookingTime, 
            string additionalNotes, string goesWithNotes, string season, string cuisine, string anevent,
            string dietaryRequirement, bool freezable, string mainFoodCategory,
            List<string>  ingredientsName, List<string> amountsName, List<string> uomName,
            List<string> commentsName, List<string> instructionsName, List<string> instructionTempName,
            List<string> originalTemperatureUoMName, string recipeIdent);

        public string ProcessImage(IFormFile file);

        public List<string> GetMainFoodCategories();

        public int GetFoodCategoryId(string foodCategoryName);

        public bool CheckSeasonValid(string season);

        public bool CheckCuisineValid(string cuisine);

        public bool CheckEventValid(string anEvent);

        public bool CheckDietaryRequirement(string dietaryRequirement);

        public bool CheckMainFoodCategory(string mainIngredient);

        public bool CheckIngredient(string mainIngredient);

        public bool CheckUnitOfMeasure(string unitOfMeasure);

        public bool DeleteRecipe(string recipeName);

        public bool DeleteRecipe(int recipeId);

        public double ConvertFromBaseToOrigUoM(double SourceAmount, string OrigUnitOfMeasure);

        public double ConvertFromOrigUoMToBase(double SourceAmount, string OrigUnitOfMeasure);

        public List<string> GetUnitsOfMeasureRange(string UofMType, string UofMSystem);

        public List<string> GetUnitsOfMeasureSymbolsForIngredientType(string IngredientName);

        public List<string> GetTemperatureUnitsOfMeasures();

        public List<string> GetFoodCategories();

        public string CheckNewIngredient(string ingredientNameSingular, string ingredientNamePlural,
            string ingredientFoodCategory, string ingredientDryOrWetMeasure);

        public Boolean AddNewIngredient(string newIngredientSingularName, string newIngredientPluralName,
            string newIngredientFoodCategoryName, string dryOrWetMeasureName);

        public int GetFoodCategoryIdFromName(string FoodCatName);

        public Boolean CheckIfIngredientAlreadyExists(string ingredientName, string scope);

        public void DeleteRecipeImageOrphans();

        public List<RecipeWithImageIngredients> GetAllTheRecipes();

        public int IncrementUserLikes(string RecipeIdent);

        public double GetTheNewUoMAmountValue(string IngredFullAmount, string NewUoMValue);

        public bool ProcessTheUpdatedImage(IFormFile recipeimage, string RecipeIdent);

        public bool UpdateStandardFields(string[][] updates, string RecipeIdent);

        public bool UpdateAddNewFields(string[][] added, string RecipeIdent);

        public bool RemoveDeletedfields(string[][] updates, string RecipeIdent);
    }
}


