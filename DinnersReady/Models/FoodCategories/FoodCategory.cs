using System.ComponentModel.DataAnnotations;


namespace DinnersReadyCore.Models.FoodCategories
{
    public class FoodCategory
    {

        [Key]
        public int FoodCategoryId { get; set; }


        [Required]
        [StringLength(50)]
        public string FoodCategoryName { get; set; }


        //Required]
        [StringLength(50)]
        public string DerivedFoodCategoryName { get; set; }

    }
}