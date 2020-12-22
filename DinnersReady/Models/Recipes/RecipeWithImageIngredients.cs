namespace DinnersReadyCore.Models.Recipes
{
    public class RecipeWithImageIngredients
    {

        public string Name { get; set; }
        public string Method { get; set; }
        public string Time { get; set; }
        public string Servings { get; set; }
        public string Category { get; set; }
        public string Derived { get; set; }
        public string Views { get; set; }
        public string Likes { get; set; }
        public string Ident { get; set; }

        public string Ingredients { get; set; }

        public string Photo { get; set; }
    }
}