using System.ComponentModel.DataAnnotations;


namespace DinnersReadyCore.Models.RecipeImages
{
    public class RecipeImage
    {

        [Key]
        public int ImageId { get; set; }


        [Required]
        [StringLength(100)]
        public string ImageName { get; set; }


        [Required]
        [StringLength(200)]        
        public string ContentType { get; set; }


        [Required]
        public byte[] Image { get; set; }


        [Required]
        [StringLength(16)]
        public string RecipeIdent { get; set; }
    }
}