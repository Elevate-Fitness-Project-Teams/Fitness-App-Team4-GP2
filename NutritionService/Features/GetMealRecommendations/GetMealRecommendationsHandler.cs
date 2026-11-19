using BuildingBlocks.Shared.Results;
using BuildingBlocks.Shared.Results.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NutritionService.Domain.Contracts;
using NutritionService.Domain.Entities;
using NutritionService.Infrastructure.Persistence.Integrations;

namespace NutritionService.Features.GetMealRecommendations
{
    public class GetMealRecommendationsHandler : IRequestHandler<GetMealRecommendationsQuery, Result<GetMealRecommendationsResponse>>
    {
        private readonly IGenericRepository<Meal> _genericRepository;
        private readonly FceClient _fceClient;

        public GetMealRecommendationsHandler(IGenericRepository<Meal> genericRepository, FceClient fceClient)
        {
            _genericRepository = genericRepository;
            _fceClient = fceClient;
        }

        public async Task<Result<GetMealRecommendationsResponse>> Handle(GetMealRecommendationsQuery request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var fceResult = await _fceClient.GetMetricsAsync(userId, cancellationToken);
            if (fceResult.IsFailure)
            {
                return Result<GetMealRecommendationsResponse>.Fail(fceResult.Errors);
            }

            var fceMetrics = fceResult.Value;

            var query = _genericRepository.GetAll(m => m.Type == request.MealType 
                                            && m.Calories <= fceMetrics.CalorieTarget 
                                            && (!request.MaxCalories.HasValue || m.Calories <= request.MaxCalories)
                                            && (!request.MinProtein.HasValue || m.Protein >= request.MinProtein));

            var totalCount = await query.CountAsync(cancellationToken);
            var pagination = request.Pagination;
            var items = await query
                .OrderBy(m => m.MealId)
                .Skip(pagination.Skip)
                .Take(pagination.PageSize)
                .Select(m => new MealRecommendationDto
                {
                    MealId = m.MealId,
                    Name = m.Name,
                    Type = m.Type.ToString(),
                    Calories = m.Calories,
                    Protein = m.Protein,
                    Carbs = m.Carbs,
                    Fats = m.Fats,
                    TagsJson = m.TagsJson
                })
                .ToListAsync(cancellationToken);

            return Result<GetMealRecommendationsResponse>.OK(new GetMealRecommendationsResponse
            {
                UserDailyGoalCalories = fceMetrics.CalorieTarget,
                RecommendedMeals = PaginatedResult<MealRecommendationDto>.Create(items, totalCount, pagination.Page, pagination.PageSize)
            });
        }
    }
}
