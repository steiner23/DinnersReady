using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using DinnersReadyCore.Models.Recipes;
using DinnersReadyCore.ViewModels.ViewRecipe;
using Microsoft.AspNetCore.Http;
using DinnersReadyCore.ViewModels.EditRecipe;

namespace DinnersReadyCore.Controllers.Edit
{

    public class EditController : Controller
    {

        private readonly IRecipeRepository _recipeRepository;


        public EditController(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }


        [HttpPost]
        public ActionResult Recipe(string ident)
        {
            EditVM editVM = new EditVM { };

            editVM.localRecipe = _recipeRepository.GetRecipeWithDetails(ident);
            editVM.MainFoodCategories = _recipeRepository.GetFoodCategories();

            return PartialView("_EditRecipe", editVM);
        }


        [HttpGet]
        public JsonResult GetUnitsOfMeasureView(string ingredient)
        {
            string localIngredient = ingredient;
            List<string> unitsOfMeasureList = _recipeRepository.GetUnitsOfMeasureSymbolsForIngredientType(localIngredient);

            return Json(unitsOfMeasureList.ToList());
        }


        public bool CheckRequiredFields(List<string> requiredFields)
        {
            bool fieldsCompleted = true;
            foreach (string temp in requiredFields)
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
        public JsonResult GetTemperatureUnitsOfMeasure()
        {
            return Json(_recipeRepository.GetTemperatureUnitsOfMeasures().ToList());
        }


        [HttpGet]
        public JsonResult GetNewUoMAmountValue(string IngredFullAmount, string NewUoMValue)
        {
            return Json(_recipeRepository.GetTheNewUoMAmountValue(IngredFullAmount, NewUoMValue));
        }


        ////////////  Update Methods ///////////////////////////////////////////

        [HttpPost]
        public bool ProcessUpdatedImage(IFormFile file, string ident)
        {
            bool updated = _recipeRepository.ProcessTheUpdatedImage(file, ident);
            return updated;
        }


        [HttpPost]
        public bool ProcessUpdatedFields(string[][] updates,  string RecipeIdent)
        {
            bool updated = _recipeRepository.UpdateStandardFields(updates, RecipeIdent);
            return updated;            
        }


        [HttpPost]
        public bool ProcessAddedFields(string[][] updates,  string RecipeIdent)
        {
            bool updated = _recipeRepository.UpdateAddNewFields(updates, RecipeIdent);
            return updated;
        }


        [HttpPost]
        public bool ProcessDeletedFields(string[][] updates,  string RecipeIdent)
        {
            bool updated = _recipeRepository.RemoveDeletedfields(updates, RecipeIdent);
            return updated;            
        }


        [HttpGet]
        public PartialViewResult GetNewIngredientView()
        {

            return PartialView("_Ingredient");
        }

    }
}