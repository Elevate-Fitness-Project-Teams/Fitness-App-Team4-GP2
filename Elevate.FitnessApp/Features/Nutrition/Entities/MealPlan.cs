namespace Elevate.FitnessApp.Features.Nutrition.Entities
{
    public class MealPlan
    {
        public int MealPlanId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double TargetCalorieRangeMin { get; set; }
        public double TargetCalorieRangeMax { get; set; }
    }
}
