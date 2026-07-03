using BuildingBlocks.Shared.Results;
using MediatR;

namespace NutritionService.Features.GetMealDetails
{
    public record GetMealDetailsQuery(int MealId) : IRequest<Result<MealDetailsResponse>>;
   
}
