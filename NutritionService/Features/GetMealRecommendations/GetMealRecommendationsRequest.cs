using BuildingBlocks.Shared.Results.Pagination;
using NutritionService.Domain.Entities;

namespace NutritionService.Features.GetMealRecommendations
{
    public record GetMealRecommendationsRequest(
        MealType MealType,
        double? MaxCalories,
        double? MinProtein,
        int Page = 1,
        int PageSize = 20
        );



}
