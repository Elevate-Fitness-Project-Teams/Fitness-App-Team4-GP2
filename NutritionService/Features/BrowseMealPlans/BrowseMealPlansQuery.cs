using BuildingBlocks.Shared.Results;
using BuildingBlocks.Shared.Results.Pagination;
using MediatR;

namespace NutritionService.Features.BrowseMealPlans
{
    public record BrowseMealPlansQuery(PaginationRequest Pagination): IRequest<Result<PaginatedResult<MealPlansResponse>>>;
    


}
