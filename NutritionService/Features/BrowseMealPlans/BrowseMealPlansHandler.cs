using BuildingBlocks.Shared.Results;
using BuildingBlocks.Shared.Results.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NutritionService.Domain.Contracts;
using NutritionService.Domain.Entities;

namespace NutritionService.Features.BrowseMealPlans
{
    public class BrowseMealPlansHandler : IRequestHandler<BrowseMealPlansQuery, Result<PaginatedResult<MealPlansResponse>>>
    {
        private readonly IGenericRepository<MealPlan> _genericRepository;

        public BrowseMealPlansHandler(IGenericRepository<MealPlan> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<Result<PaginatedResult<MealPlansResponse>>> Handle(BrowseMealPlansQuery request, CancellationToken cancellationToken)
        {
            var query = _genericRepository.GetAll();

            var totalCount = await query.CountAsync();
            var pagination = request.Pagination;

            var items = await query
                .OrderBy(x=>x.MealPlanId)
                .Skip(pagination.Skip)
                .Take(pagination.PageSize)
                .Select(mp => new MealPlansResponse
                (
                    mp.Name,
                    mp.Description,
                    mp.TargetCalorieRangeMin,
                    mp.TargetCalorieRangeMax,
                    mp.MealPlanItems.Select(item => new MealPlanItemResponse
                    (
                        item.Meal.Name ,
                        item.Meal.Calories,
                        item.DayOfWeek,
                        item.MealTime
                    )).ToList()
                )).ToListAsync();
                
            return PaginatedResult<MealPlansResponse>.Create(items, totalCount, pagination.Page, pagination.PageSize);

        }
    }
}
