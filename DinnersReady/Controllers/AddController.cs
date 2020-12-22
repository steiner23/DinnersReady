using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DinnersReadyCore.Models;
using DinnersReadyCore.Models.Recipes;
using DinnersReadyCore.Models.Ingredients;
using DinnersReadyCore.ViewModels.AddRecipe;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Builder;
using System.Reflection.Metadata.Ecma335;



namespace DinnersReadyCore.Controllers.Add
{
    public class AddController : Controller
    {

        private readonly bool recipeAdded = false;
        private readonly IRecipeRepository _recipeRepository;


        public AddController(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }
        

        [HttpGet]
        public ActionResult Recipe()
        {
            AddVM addViewModel = new AddVM { };
  
            addViewModel.RecipeAddedSuccess = recipeAdded;
            addViewModel.MainFoodCategories = _recipeRepository.GetMainFoodCategories();

            return PartialView("_AddRecipe", addViewModel);
        }


        [HttpPost]
        public JsonResult ProcessNewRecipe(string recipeName, string description,
            string course, string cookingMethod, string servings, string cookingTime,
            List<string>  ingredientsName, List<string> amountsName, List<string> uomName,
            List<string> commentsName, List<string> instructionsName, List<string> instructionTempName,
            List<string> originalTemperatureUoMName,
            string additionalNotes, string goesWithNotes, string season, string cuisine,
            string anevent, string dietaryRequirement, bool freezable, string mainFoodCategory,
            string recipeIdt)
        {
           
            return Json(_recipeRepository.ProcessRecipe(recipeName, description,
                course, cookingMethod, servings, cookingTime, additionalNotes,
                goesWithNotes, season, cuisine, anevent, dietaryRequirement,
                freezable, mainFoodCategory, ingredientsName, amountsName, uomName, commentsName,
                instructionsName, instructionTempName, originalTemperatureUoMName, recipeIdt));     
        }


        [HttpPost] 
        public string ProcessNewImage(IFormFile file)
        {
            string recipeIdent = _recipeRepository.ProcessImage(file);
            return recipeIdent;
        }

   
        public bool CheckRequiredFields(List<string> requiredFields)
        {
            bool fieldsCompleted = true;
            foreach(string temp in requiredFields)
            {
                if (String.IsNullOrEmpty(temp))
                {
                    fieldsCompleted = false;
                }
            }
            return fieldsCompleted;
        }


        [HttpGet]
        public JsonResult GetIngredients(string ingredient)
        {
            string localIngredient = ingredient;
            var numberOfIngredients = 20;
            List<string> ingredientsLikeList = _recipeRepository.GetIngredientsLike(localIngredient, numberOfIngredients);

            return Json(ingredientsLikeList.ToList());
        }


        [HttpGet]
        public JsonResult GetUnitsOfMeasureAdd(string ingredient)
        {
            string localIngredient = ingredient;
            List<string> unitsOfMeasureList = _recipeRepository.GetUnitsOfMeasureSymbolsForIngredientType(localIngredient);

            return Json(unitsOfMeasureList.ToList());
        }


        [HttpGet]
        public JsonResult GetTemperatureUnitsOfMeasure()
        {
            return Json(_recipeRepository.GetTemperatureUnitsOfMeasures().ToList());
        }


        [HttpGet]
        public JsonResult GetFoodCategories()
        {
            List<string> foodCategoryNames = _recipeRepository.GetFoodCategories().ToList();
            return Json(foodCategoryNames);
        }


        [HttpGet]
        public JsonResult ProcessNewIngredient(string ingredientNameSingular, string ingredientNamePlural,
            string ingredientFoodCategory, string ingredientDryOrWetMeasure)
        {
            return Json(_recipeRepository.CheckNewIngredient(ingredientNameSingular, ingredientNamePlural,
                ingredientFoodCategory, ingredientDryOrWetMeasure));
        }


        [HttpGet]
        public PartialViewResult GetNewIngredientView()
        {

            return PartialView("_Ingredient");
        }


        [HttpPost]
        public Boolean AddIngredient(string newIngredientSingularName, string newIngredientPluralName,
            string newIngredientFoodCategoryName, string dryOrWetMeasureName)
        {
            Boolean IngredientAdded = _recipeRepository.AddNewIngredient(newIngredientSingularName,
                newIngredientPluralName, newIngredientFoodCategoryName, dryOrWetMeasureName);

            return IngredientAdded;
        }


        [HttpGet]
        public void DeleteImageOrphans()
        {
            _recipeRepository.DeleteRecipeImageOrphans();
        }


    } // close class
}  // close namespace