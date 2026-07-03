using BuildingBlocks.Shared.Results.Pagination;

namespace NutritionService.Features.GetMealRecommendations
{
    public class GetMealRecommendationsResponse
    {
        public double UserDailyGoalCalories { get; set; }
        public PaginatedResult<MealRecommendationDto> RecommendedMeals { get; set; } = default!;

    }

    public class MealRecommendationDto
    {
        public int MealId { get; set; }

        public string Name { get; set; } = default!;

        public string Type { get; set; } = default!;

        public double Calories { get; set; }

        public double Protein { get; set; }

        public double Carbs { get; set; }

        public double Fats { get; set; }

        public string? TagsJson { get; set; }
    }
}