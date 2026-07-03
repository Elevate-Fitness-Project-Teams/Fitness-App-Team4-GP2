namespace NutritionService.Domain.Entities
{
    public class MealPlan
    {
        public int MealPlanId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double TargetCalorieRangeMin { get; set; }
        public double TargetCalorieRangeMax { get; set; }
        public ICollection<MealPlanItem> MealPlanItems { get; set; } = default!;
    }
}
