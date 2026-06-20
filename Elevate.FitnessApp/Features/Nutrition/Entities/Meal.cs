namespace Elevate.FitnessApp.Features.Nutrition.Entities
{
    public class Meal
    {
        public int MealId { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Carbs { get; set; }
        public double Fats { get; set; }
        public string IngredientsJson { get; set; } = null!;
        public string InstructionsJson { get; set; } = null!;
        public string? VariationsJson { get; set; }
        public string AllergensJson { get; set; } = null!;
        public string TagsJson { get; set; } = null!;
    }
}