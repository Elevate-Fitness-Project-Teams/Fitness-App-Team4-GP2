using BuildingBlocks.Shared.Results;
using BuildingBlocks.Shared.Results.Pagination;
using MediatR;
using NutritionService.Domain.Entities;

namespace NutritionService.Features.GetMealRecommendations
{
    public record GetMealRecommendationsQuery(
        Guid UserId,
        MealType MealType ,
        double? MaxCalories  , 
        double? MinProtein ,
        PaginationRequest Pagination
        ) : IRequest<Result<GetMealRecommendationsResponse>>;
    
}
