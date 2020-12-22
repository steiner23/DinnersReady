using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DinnersReadyCore.Models.RecipeInstructions
{
    public class RecipeInstruction
    {

        [Key]
        public int InstructionId { get; set; }


        //The internal ident
        [Required]
        [StringLength(16)]
        public string RecipeInstructionIdent { get; set; }


        //The Recipe Id to which this instruction belongs
        [Required]
        public int RecipeId { get; set; }
        [ForeignKey("RecipeId")]
        public virtual Recipe Recipes { get; set; }


        //The Id for this Instruction Step, of which there may be many for each recipe
        //This field would be used to order the Instruction Steps per recipe
        [Required]
        public int InstructionStepId { get; set; }


        //The actual Instruction details from the user
        [Required]
        [StringLength(500)]
        public string Description { get; set; }


        //the Temperature value will be stored here in Degrees Celsius
        //if the user enters a value in another format it will be converted using values
        //from the UnitsOfTemperature table
        public double TemperatureValue { get; set; }


        //A field for the system to record the Original Unit of Measure used
        //for this Temperature
        [StringLength(20)]       
        public string OriginalTemperatureUnitOfMeasure { get; set; }

    }
}