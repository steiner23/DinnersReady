using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using DinnersReadyCore.Models.Recipes;
using DinnersReadyCore.ViewModels.ViewRecipe;
using Microsoft.AspNetCore.Http;

namespace DinnersReadyCore.Controllers.View
{

    public class ViewController : Controller
    {

        private readonly IRecipeRepository _recipeRepository;


        public ViewController(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }


        [HttpPost]
        public ActionResult Recipe(string ident)
        {
            ViewVM viewVM = new ViewVM { };

            viewVM.localRecipe = _recipeRepository.GetRecipeWithDetails(ident);
            viewVM.MainFoodCategories = _recipeRepository.GetFoodCategories();

            return PartialView("_ViewRecipe", viewVM);
        }


        [HttpGet]
        public JsonResult GetUnitsOfMeasureView(string ingredient)
        {
            string localIngredient = ingredient;
            List<string> unitsOfMeasureList = _recipeRepository.GetUnitsOfMeasureSymbolsForIngredientType(localIngredient);

            return Json(unitsOfMeasureList.ToList());
        }


        [HttpPost]
        public JsonResult AddUserLike(string RecipeId)
        {
            int userLikesTotal = _recipeRepository.IncrementUserLikes(RecipeId); 
            return Json(userLikesTotal);
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


    } // end class

} // end namespace






