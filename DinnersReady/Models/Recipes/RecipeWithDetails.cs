using System;
using System.Collections.Generic;


namespace DinnersReadyCore.Models.Recipes
{
    public class RecipeWithDetails : Recipe
    {

        // variables for Image information
        public string Photo { get; set; }


        // variables for Food Categories and Derived Food Categories
        public string FoodCategory { get; set; }

        public string DerivedCategory { get; set; }


        //Variables for Ingredient information

        public List<string> IngredientNames { get; set; }

        public List<double> IngredientBaseAmounts { get; set; }

        public List<double> IngredientViewAmounts { get; set; }

        public List<string> OriginalUnitOfMeasures { get; set; }

        public List<string> IngredientComments { get; set; }

        public List<string> RecipeIngredientIdents { get; set; }


        //Variables for Instruction information

        public List<string> InstructionDescriptions { get; set; }

        public List<double> TemperatureBaseValues { get; set; }

        public List<double> TemperatureValues { get; set; }

        public List<String> OriginalTemperatureUnitOfMeasure { get; set; }

        public List<string> RecipeInstructionIdents { get; set; }

    }
}


