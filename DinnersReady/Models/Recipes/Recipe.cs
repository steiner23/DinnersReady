using System;
using System.ComponentModel.DataAnnotations;



namespace DinnersReadyCore.Models
{
    public class Recipe
    {

        [Key]
        public int RecipeId { get; set; }


        //Recipe Name
        [Required(ErrorMessage = "Required: Name")]
        [StringLength(150)]
        public string Name { get; set; }


        //Recipe Description
        [StringLength(300)]
        public string Description { get; set; }


        //which course, breakfast, lunch, dinner
        [Required(ErrorMessage = "Required: Meal")]
        [StringLength(30)]
        public string Course { get; set; }


        //primary cooking method: oven, hob, slow-cooker
        [Required(ErrorMessage = "Required: Cooking Method")]
        [StringLength(30)]
        public string CookMethod { get; set; }


        //number of servings 1, 2, 3, 4 ... 100  
        [Required(ErrorMessage = "Required: Servings")]
        public int Servings { get; set; }


        //prep time ranges, <30mins, 30-60mins, 60-90mins ...
        [Required(ErrorMessage = "Required: Cooking Time")]
        [StringLength(30)]
        public string CookingTime { get; set; }


        [Required]
        public DateTime GenDate { get; set; }


        [Required]
        [StringLength(16)]
        public string RecipeIdent { get; set; }


        // ----------------------------------------
        //      OPTIONAL FIELDS
        // ----------------------------------------


        //Recipe Notes for user, for info not recorded elsewhere
        [StringLength(1000)]
        public string AdditionalNotes { get; set; }


        //Serving suggestions
        [StringLength(150)]
        public string GoesWith { get; set; }


        //how many times this recipie's details are viewed
        public int? ViewCount { get; set; }


        //to record user likes
        public int? UserLikes { get; set; }


        //Seasons: "Spring", "Summer", "Autumn", "Winter", "All Year Round"
        [StringLength(30)]
        public string Season { get; set; }


        //Cusine type: "British", "Chinese", "French", "Indian", "Irish", "Italian", "Spanish", "Thai", "Other"
        [StringLength(30)]
        public string Cuisine { get; set; }


        //Events: "Christmas", "Birthday", "New Year", "Easter", "Party", "Barbeque", "Other"
        [StringLength(30)]
        public string Event { get; set; }


        //Dietary Requirements: None, Kosher, Gluten-free, Halal, Lactose-free, Vegan, Vegetarian
        [StringLength(30)]
        public string DietaryRequirement { get; set; }


        //Can it be frozen: 'Y' or 'N'
        public bool? Freezable { get; set; }


        //Future Use: When a Recipe is manually enetered it will be flagged for review by an Admin
        //ReviewedStatus is false before review and true afterwards
        //a null value implies the recipe was hard-coded at setup
        public bool? ReviewedStatus { get; set; }


        public int FoodCategoryId { get; set; }

    }
}

