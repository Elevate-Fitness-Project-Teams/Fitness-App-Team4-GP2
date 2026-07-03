using BuildingBlocks.Shared.Results;
using MediatR;
using NutritionService.Domain.Contracts;
using NutritionService.Domain.Entities;
using System.Text.Json;

namespace NutritionService.Features.GetMealDetails
{
    public class GetMealDetailsHandler : IRequestHandler<GetMealDetailsQuery, Result<MealDetailsResponse>>
    {
        private readonly IGenericRepository<Meal> _mealRepository;

        public GetMealDetailsHandler(IGenericRepository<Meal> mealRepository)
        {
            _mealRepository = mealRepository;
        }
        public async Task<Result<MealDetailsResponse>> Handle(GetMealDetailsQuery request, CancellationToken cancellationToken)
        {
            var meal = await _mealRepository.GetByIdAsync(request.MealId);
            if (meal is null)
                return Error.NotFound("NotFound", "RES_MEAL_NOT_FOUND");


            return new MealDetailsResponse
            (
                meal.Name,
                meal.Type,
                meal.Calories,
                meal.Protein,
                meal.Carbs,
                meal.Fats,
                JsonDocument.Parse(meal.IngredientsJson).RootElement.Clone(),
                JsonDocument.Parse(meal.InstructionsJson).RootElement.Clone(),
                string.IsNullOrWhiteSpace(meal.VariationsJson) ? null: JsonDocument.Parse(meal.VariationsJson).RootElement.Clone(),
                JsonDocument.Parse(meal.AllergensJson).RootElement.Clone(),
                JsonDocument.Parse(meal.TagsJson).RootElement.Clone()


            );
           
        }
    }
}
