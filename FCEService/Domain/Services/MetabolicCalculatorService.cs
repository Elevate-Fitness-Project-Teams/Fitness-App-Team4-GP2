using FCEService.Domain.Enums;

using static FCEService.Domain.Services.MetabolicConstants;

namespace FCEService.Domain.Services
{
    
    public sealed class MetabolicCalculatorService : IMetabolicCalculatorService
    {
        public double CalculateBmr(double weightKg, double heightCm, int age, Gender gender)
        {
            var baseValue = (WeightCoefficient * weightKg)
                           + (HeightCoefficient * heightCm)
                           - (AgeCoefficient * age);

            return gender switch
            {
                Gender.Male => Math.Round(baseValue + MaleConstant, RoundingDecimalPlaces),
                Gender.Female => Math.Round(baseValue + FemaleConstant, RoundingDecimalPlaces),
                _ => 0d
            };
        }

        public double CalculateTdee(double bmr, ActivityLevel activityLevel) => activityLevel switch
        {
            ActivityLevel.Rookie => Math.Round(bmr * RookieActivityFactor, RoundingDecimalPlaces),
            ActivityLevel.Beginner => Math.Round(bmr * BeginnerActivityFactor, RoundingDecimalPlaces),
            ActivityLevel.Intermediate => Math.Round(bmr * IntermediateActivityFactor, RoundingDecimalPlaces),
            ActivityLevel.Advance => Math.Round(bmr * AdvanceActivityFactor, RoundingDecimalPlaces),
            ActivityLevel.TrueBeast => Math.Round(bmr * TrueBeastActivityFactor, RoundingDecimalPlaces),
            _ => 0d
        };

        public double CalculateCalorieTarget(double tdee, FitnessGoal goal) => goal switch
        {
            FitnessGoal.LoseWeight => Math.Round(tdee + LoseWeightDelta, RoundingDecimalPlaces),
            FitnessGoal.GetFitter => Math.Round(tdee, RoundingDecimalPlaces),
            FitnessGoal.GainWeight => Math.Round(tdee + GainWeightDelta, RoundingDecimalPlaces),
            FitnessGoal.GainMoreFlexible => Math.Round(tdee + GainMoreFlexibleDelta, RoundingDecimalPlaces),
            FitnessGoal.LearnTheBasic => Math.Round(tdee, RoundingDecimalPlaces),
            _ => 0d
        };

        public PlanStatus DetermineStatus(double calorieTarget) => calorieTarget switch
        {
            <= 0 => PlanStatus.None,
            <= WeakStatusMaxThreshold => PlanStatus.Weak,
            <= NormalStatusMaxThreshold => PlanStatus.Normal,
            _ => PlanStatus.Hard
        };
    }
}