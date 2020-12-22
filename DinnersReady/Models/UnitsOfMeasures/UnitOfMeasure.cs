using System.ComponentModel.DataAnnotations;


namespace DinnersReadyCore.Models.UnitsOfMeasures
{
    public class UnitOfMeasure
    {

        [Key]
        public int UnitOfMeasureId { get; set; }


        //Unit of Measure Symbol
        [Required]
        [StringLength(10)]
        public string UnitOfMeasureSymbol { get; set; }


        //Unit of Measure Name
        [Required]
        [StringLength(20)]
        public string UnitOfMeasureName { get; set; }


        //Unit of Measure Name in Plural form
        [Required]
        [StringLength(20)]
        public string UnitOfMeasureNamePlural { get; set; }


        //The Id, in this same UnitOfMeasure table, for the base unit of measure
        //only the Gram(g), Millilitre(ml), Degree Celsius and Millimetre(cm) are Base Measures
        [Required]
        public int BaseUnitOfMeasureId { get; set; }


        //Rate of Ingredient's Conversion by Multipliation to its Base Unit Of Measure 
        [Required]
        public double MultiplyConversionToBaseRate { get; set; }


        //Rate of Conversion by Multipliation from the Base Unit Of Measure to the Ingredients
        [Required]
        public double MultiplyConversionToIngredientRate { get; set; }


        //Indicates whether Measure is a Base one
        //only the Gram(g), Millilitre(ml), Degree Celsius and Millimetre(cm) are Base Measures
        [Required]
        public bool IsBaseUnitOfMeasure { get; set; }


        //Defines whether a Measure or Temperature is Metric or Imperial
        //and therefore when it's available based upon user seleciton
        [Required]
        [StringLength(30)]
        public string SystemOfMeasureName { get; set; }


        //Defines whether the Unit of Measure is Dry or Wet
        //is n/a for Lengths plus Temperatures
        [Required]
        [StringLength(15)]
        public string DryOrWetUnitOfMeasure { get; set; }


        //Defines the Unit of masure Display Name
        [Required]
        [StringLength(30)]
        public string DisplayUomName { get; set; }

    }
}