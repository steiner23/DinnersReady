using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DinnersReadyCore.Models.Ingredients;


namespace DinnersReadyCore.Models.RecipeIngredients
{
    public class RecipeIngredient
    {

        [Key]
        public int RecipeIngredientId { get; set; }


        //Recipe Id key
        [Required]
        public int RecipeId { get; set; }
        [ForeignKey("RecipeId")]
        public virtual Recipe Recipes { get; set; }


        //The internal ident
        [Required]
        [StringLength(16)]
        public string RecipeIngredientIdent { get; set; }


        //Ingredient Id
        [Required]
        public int IngredientId { get; set; }
        [ForeignKey("IngredientId")]
        public virtual Ingredient Ingredients { get; set; }


        //Amount of Ingredient
        [Required]
        public double Amount { get; set; }


        //A field for the system to record the Original Unit of Measure used
        //for this Recipe Ingredient
        [Required]
        [StringLength(20)]
        public string OriginalUnitOfMeasure { get; set; }


        [StringLength(100)]
        public string Comment { get; set; }

    }
}