using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using DinnersReadyCore.Models.Recipes;



namespace DinnersReadyCore.Controllers.List
{
    public class ListController : Controller
    {

        // private readonly bool recipeAdded = false;
        private readonly IRecipeRepository _recipeRepository;


        public ListController(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }


        // GET: List
        public ActionResult _AllRecipes()
        {
            return PartialView();
        }


        [HttpPost]
        public JsonResult GetAllRecipes()
        {
            return Json(_recipeRepository.GetAllTheRecipes());
        }

    }
}