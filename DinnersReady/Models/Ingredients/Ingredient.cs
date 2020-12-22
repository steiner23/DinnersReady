using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DinnersReadyCore.Models.FoodCategories;


namespace DinnersReadyCore.Models.Ingredients
{
    public class Ingredient
    {
        [Key]
        public int IngredientId { get; set; }


        //Ingredient Name in the Singular form
        [Required]
        [StringLength(50)]
        public string NameSingular { get; set; }


        //Ingredient Name in the Plural form
        [Required]
        [StringLength(50)]
        public string NamePlural { get; set; }


        //Food Category: Beef, Pork, Nuts, Breads, Cereals...
        //so users can search and find this ingredient
        //when the Recipe Ingredients display they will be ordered ny Food Category then Name
        [Required]
        public int FoodCategoryId { get; set; }
        [ForeignKey("FoodCategoryId")]
        public virtual FoodCategory FoodCategorys { get; set; }


        //This holds the Type of the Unit Of Measure, namely:
        //    Dry or Liquid or Size or Temp
        // Dry Measure (g, kg, oz, lb, tsp(5g approx), tbsp(15g approx)), 
        // Liquid Measure (L, ml, fl oz, pints, tsp(5ml), tbsp(15ml)),
        // Size Measure (cm, inch),
        // Temperature Measure (C, F, Gas mark)
        //It tells us which range of potential measure may need to be supplied for 
        //the ingredient when it's being viewed
        [Required]
        [StringLength(15)]
        public string TypeOfMeasureName { get; set; }


        //Future Use: When an Ingredient is manually enetered it will be flagged for Admin review
        //ReviewedStatus is false before review and true afterwards
        //a null value implies the Ingredient was hard-coded at setup
        public bool? ReviewedStatus { get; set; }


        //Conversion rate Gram / Millilitre to Teaspoon (UK) for this Ingredient
        public double OneGramToTeaspoonUK { get; set; }


        //Conversion rate Teaspoon (UK) to Gram / Millilitre for this Ingredient
        public double OneTeaspoonUKToGram { get; set; }


    }
}