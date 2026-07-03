using BuildingBlocks.Shared.Results;
using BuildingBlocks.Shared.Results.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NutritionService.Domain.Contracts;
using NutritionService.Domain.Entities;
using NutritionService.Features.BrowseMealPlans;

namespace NutritionService.Features.GetMealPlansByCalorie
{
    public class GetMealPlansByCalorieHandler : IRequestHandler<GetMealPlansByCalorieQuery, Result<PaginatedResult<MealPlansResponse>>>
    {
        private readonly IGenericRepository<MealPlan> _genericRepository;

        public GetMealPlansByCalorieHandler(IGenericRepository<MealPlan> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<Result<PaginatedResult<MealPlansResponse>>> Handle(GetMealPlansByCalorieQuery request, CancellationToken cancellationToken)
        {
            var calorie = request.Calorie;

            if (calorie >= 0)
                return Error.Validation("ValidationError", "VAL_REQUIRED_FIELD");
            


            var query = _genericRepository.GetAll(x=> x.TargetCalorieRangeMin<=calorie && x.TargetCalorieRangeMax>=calorie);

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
                        item.Meal.Name,
                        item.Meal.Calories,
                        item.DayOfWeek,
                        item.MealTime
                    )).ToList()
                )).ToListAsync();

            return PaginatedResult<MealPlansResponse>.Create(items, totalCount, pagination.Page, pagination.PageSize);

        }
    }
}
