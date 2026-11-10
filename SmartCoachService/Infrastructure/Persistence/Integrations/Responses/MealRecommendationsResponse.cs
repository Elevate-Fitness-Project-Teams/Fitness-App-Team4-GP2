using BuildingBlocks.Shared.Results.Pagination;

namespace SmartCoachService.Infrastructure.Persistence.Integrations.Responses
{
    // Mirrors NutritionService.GetMealRecommendationsResponse returned by
    // GET /api/v1/nutrition/recommendations/{userId}.
    public record MealRecommendationsResponse(
        double UserDailyGoalCalories,
        PaginatedResult<MealRecommendationDto> RecommendedMeals);

    public record MealRecommendationDto(
        int MealId,
        string Name,
        string Type,
        double Calories,
        double Protein,
        double Carbs,
        double Fats,
        string? TagsJson);
}
