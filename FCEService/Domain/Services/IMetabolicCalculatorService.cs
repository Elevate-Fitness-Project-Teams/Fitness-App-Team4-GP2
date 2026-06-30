using FCEService.Domain.Enums;

namespace FCEService.Domain.Services
{
    public interface IMetabolicCalculatorService
    {
        double CalculateBmr(double weightKg, double heightCm, int age, Gender gender);

        double CalculateTdee(double bmr, ActivityLevel activityLevel);
       
        double CalculateCalorieTarget(double tdee, FitnessGoal goal);
        PlanStatus DetermineStatus(double calorieTarget);
    }
}
