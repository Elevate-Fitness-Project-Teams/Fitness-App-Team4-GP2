using NutritionService.Domain.Entities;
using System.Text.Json;

namespace NutritionService.Features.GetMealDetails
{
    public record MealDetailsResponse(
        string Name,
        MealType Type,
        double Calories,
        double Protein,
        double Carbs,
        double Fats,
        JsonElement Ingredients,
        JsonElement Instructions,
        JsonElement? Variations,
        JsonElement Allergens,
        JsonElement Tags
    );
    
}