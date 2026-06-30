using FCEService.Domain.Enums;

namespace FCEService.Domain.Services
{
    public sealed class MetabolicCalculatorService : IMetabolicCalculatorService
    {
        public double CalculateBmr(double weightKg, double heightCm, int age, Gender gender)
        {
            var baseValue = (10 * weightKg) + (6.25 * heightCm) - (5 * age);

            return gender switch
            {
                Gender.Male => Math.Round(baseValue + 5, 2),
                Gender.Female => Math.Round(baseValue - 161, 2),
                _ => 0d 
            };
        }

        public double CalculateTdee(double bmr, ActivityLevel activityLevel) => activityLevel switch
        {
            ActivityLevel.Rookie => Math.Round(bmr * 1.200, 2),
            ActivityLevel.Beginner => Math.Round(bmr * 1.375, 2),
            ActivityLevel.Intermediate => Math.Round(bmr * 1.550, 2),
            ActivityLevel.Advance => Math.Round(bmr * 1.725, 2),
            ActivityLevel.TrueBeast => Math.Round(bmr * 1.900, 2),
            _ => 0d
        };

        public double CalculateCalorieTarget(double tdee, FitnessGoal goal) => goal switch
        {
            FitnessGoal.LoseWeight => Math.Round(tdee - 500, 2),
            FitnessGoal.GetFitter => Math.Round(tdee, 2),
            FitnessGoal.GainWeight => Math.Round(tdee + 300, 2),
            FitnessGoal.GainMoreFlexible => Math.Round(tdee + 150, 2),
            FitnessGoal.LearnTheBasic => Math.Round(tdee, 2),
            _ => 0d
        };

        public PlanStatus DetermineStatus(double calorieTarget) => calorieTarget switch
        {
            <= 0 => PlanStatus.None,  
            <= 1800 => PlanStatus.Weak,
            <= 2500 => PlanStatus.Normal,
            _ => PlanStatus.Hard
        };
    }
}