using BuildingBlocks.Shared.Results;
using BuildingBlocks.Shared.Results.Pagination;
using MediatR;
using NutritionService.Features.BrowseMealPlans;

namespace NutritionService.Features.GetMealPlansByCalorie
{
    public record GetMealPlansByCalorieQuery(double Calorie , PaginationRequest Pagination) : IRequest<Result<PaginatedResult<MealPlansResponse>>>;
    

}
