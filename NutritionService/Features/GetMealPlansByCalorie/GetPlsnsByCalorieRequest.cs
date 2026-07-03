namespace NutritionService.Features.GetMealPlansByCalorie
{
    public record GetPlsnsByCalorieRequest(double Calorie , int Page = 1, int PageSize = 20);

}
