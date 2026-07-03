using BuildingBlocks.Shared.Results;
using BuildingBlocks.Shared.Results.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NutritionService.Domain.Contracts;
using NutritionService.Domain.Entities;
using NutritionService.Infrastructure.Persistence.Integrations;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace NutritionService.Features.GetMealRecommendations
{
    public class GetMealRecommendationsHandler : IRequestHandler<GetMealRecommendationsQuery, Result<GetMealRecommendationsResponse>>
    {
        private readonly IGenericRepository<Meal> _genericRepository;
        private readonly FceClient _fceClient;

        public GetMealRecommendationsHandler(IGenericRepository<Meal> genericRepository , FceClient fceClient)
        {
            _genericRepository = genericRepository;
            _fceClient = fceClient;
        }
        public async Task<Result<GetMealRecommendationsResponse>> Handle(GetMealRecommendationsQuery request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var FceMetrics = await _fceClient.GetMetricsAsync(userId , cancellationToken);
            if(FceMetrics is null)
                return Error.Validation("ValidationError" , "FCE_METRICS_NOT_CALCULATED");


            var query = _genericRepository.GetAll(m => m.Type == request.MealType 
                                            && m.Calories <= FceMetrics.CalorieTarget 
                                            && (!request.MaxCalories.HasValue || m.Calories <= request.MaxCalories)
                                            && (!request.MinProtein.HasValue || m.Protein >= request.MinProtein));

            var totalCount = await query.CountAsync();
            var pagination = request.Pagination;
            var items = await query
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
                .ToListAsync();

            return new GetMealRecommendationsResponse
            {
                UserDailyGoalCalories = FceMetrics.CalorieTarget,
                RecommendedMeals = PaginatedResult<MealRecommendationDto>.Create(items, totalCount, pagination.Page, pagination.PageSize)
            };




        }
    }
}
