namespace NutritionService.Features.BrowseMealPlans
{
    public record MealPlansResponse(
        string Name,
        string Description,
        double TargetCalorieRangeMin,
        double TargetCalorieRangeMax,
        List<MealPlanItemResponse> Items
        );

    public record MealPlanItemResponse(
        string MealName,
        double MealCalories,
        string DayOfWeek,
        string MealTime
        );
    
}