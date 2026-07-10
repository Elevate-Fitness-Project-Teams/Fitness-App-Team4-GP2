using FCEService.Domain.Entities;
using FCEService.Domain.Enums;

namespace FCEService.Infrastructure.Persistence.Seed
{
    
        /// <summary>
        /// PlanId يتبع Convention: {goal}-{status} (kebab-case). ده اقتراح مبدئي
        /// لحد ما يتأكد نفس القيم بالظبط مع فريق Workout Service — لو اتغيروا،
        /// التعديل هنا بس Data مش Logic.
        /// القيم (MinCalorie/MaxCalorie/EstimatedDuration/WorkoutsPerWeek/ProgramType)
        /// دي Placeholder منطقي لغرض الاختبار — محتاجة تأكيد من فريق البيزنس/التغذية
        /// قبل الإطلاق الفعلي.
        /// </summary>
        public static class FitnessPlanConfigSeed
        {
            public static IEnumerable<FitnessPlanConfig> GetSeedData() => new[]
            {
            // ---- Lose Weight ----
            new FitnessPlanConfig
            {
                PlanId = "lose-weight-weak", PlanName = "Gentle Fat Loss",
                Description = "Low-intensity plan for users with low calorie targets aiming to lose weight.",
                Goal = FitnessGoal.LoseWeight, Status = PlanStatus.Weak,
                MinCalorie = 0, MaxCalorie = 1800, EstimatedDuration = "8 weeks",
                WorkoutsPerWeek = 3, ProgramType = "Low-Impact Cardio"
            },
            new FitnessPlanConfig
            {
                PlanId = "lose-weight-normal", PlanName = "Balanced Fat Loss",
                Description = "Moderate-intensity plan for users with a normal calorie target aiming to lose weight.",
                Goal = FitnessGoal.LoseWeight, Status = PlanStatus.Normal,
                MinCalorie = 1801, MaxCalorie = 2500, EstimatedDuration = "8 weeks",
                WorkoutsPerWeek = 4, ProgramType = "Cardio + Strength"
            },
            new FitnessPlanConfig
            {
                PlanId = "lose-weight-hard", PlanName = "Intensive Fat Loss",
                Description = "High-intensity plan for users with a high calorie target aiming to lose weight.",
                Goal = FitnessGoal.LoseWeight, Status = PlanStatus.Hard,
                MinCalorie = 2501, MaxCalorie = double.MaxValue, EstimatedDuration = "10 weeks",
                WorkoutsPerWeek = 5, ProgramType = "HIIT + Strength"
            },

            // ---- Get Fitter ----
            new FitnessPlanConfig
            {
                PlanId = "get-fitter-weak", PlanName = "Foundation Fitness",
                Description = "Entry-level general fitness plan for low calorie targets.",
                Goal = FitnessGoal.GetFitter, Status = PlanStatus.Weak,
                MinCalorie = 0, MaxCalorie = 1800, EstimatedDuration = "6 weeks",
                WorkoutsPerWeek = 3, ProgramType = "Full-Body Basics"
            },
            new FitnessPlanConfig
            {
                PlanId = "get-fitter-normal", PlanName = "General Fitness",
                Description = "Balanced general fitness plan for normal calorie targets.",
                Goal = FitnessGoal.GetFitter, Status = PlanStatus.Normal,
                MinCalorie = 1801, MaxCalorie = 2500, EstimatedDuration = "8 weeks",
                WorkoutsPerWeek = 4, ProgramType = "Full-Body + Cardio"
            },
            new FitnessPlanConfig
            {
                PlanId = "get-fitter-hard", PlanName = "Advanced Fitness",
                Description = "Advanced general fitness plan for high calorie targets.",
                Goal = FitnessGoal.GetFitter, Status = PlanStatus.Hard,
                MinCalorie = 2501, MaxCalorie = double.MaxValue, EstimatedDuration = "10 weeks",
                WorkoutsPerWeek = 5, ProgramType = "Full-Body + HIIT"
            },

            // ---- Gain Weight ----
            new FitnessPlanConfig
            {
                PlanId = "gain-weight-weak", PlanName = "Lean Bulk Starter",
                Description = "Entry-level muscle gain plan for low calorie targets.",
                Goal = FitnessGoal.GainWeight, Status = PlanStatus.Weak,
                MinCalorie = 0, MaxCalorie = 1800, EstimatedDuration = "10 weeks",
                WorkoutsPerWeek = 3, ProgramType = "Strength Foundations"
            },
            new FitnessPlanConfig
            {
                PlanId = "gain-weight-normal", PlanName = "Lean Bulk",
                Description = "Balanced muscle gain plan for normal calorie targets.",
                Goal = FitnessGoal.GainWeight, Status = PlanStatus.Normal,
                MinCalorie = 1801, MaxCalorie = 2500, EstimatedDuration = "12 weeks",
                WorkoutsPerWeek = 4, ProgramType = "Hypertrophy Strength"
            },
            new FitnessPlanConfig
            {
                PlanId = "gain-weight-hard", PlanName = "Mass Gain",
                Description = "Advanced muscle gain plan for high calorie targets.",
                Goal = FitnessGoal.GainWeight, Status = PlanStatus.Hard,
                MinCalorie = 2501, MaxCalorie = double.MaxValue, EstimatedDuration = "12 weeks",
                WorkoutsPerWeek = 5, ProgramType = "Heavy Strength + Volume"
            },

            // ---- Gain More Flexible ----
            new FitnessPlanConfig
            {
                PlanId = "gain-more-flexible-weak", PlanName = "Mobility Starter",
                Description = "Entry-level flexibility/mobility plan for low calorie targets.",
                Goal = FitnessGoal.GainMoreFlexible, Status = PlanStatus.Weak,
                MinCalorie = 0, MaxCalorie = 1800, EstimatedDuration = "6 weeks",
                WorkoutsPerWeek = 3, ProgramType = "Mobility + Stretching"
            },
            new FitnessPlanConfig
            {
                PlanId = "gain-more-flexible-normal", PlanName = "Flexibility & Mobility",
                Description = "Balanced flexibility/mobility plan for normal calorie targets.",
                Goal = FitnessGoal.GainMoreFlexible, Status = PlanStatus.Normal,
                MinCalorie = 1801, MaxCalorie = 2500, EstimatedDuration = "8 weeks",
                WorkoutsPerWeek = 4, ProgramType = "Yoga + Mobility"
            },
            new FitnessPlanConfig
            {
                PlanId = "gain-more-flexible-hard", PlanName = "Advanced Mobility",
                Description = "Advanced flexibility/mobility plan for high calorie targets.",
                Goal = FitnessGoal.GainMoreFlexible, Status = PlanStatus.Hard,
                MinCalorie = 2501, MaxCalorie = double.MaxValue, EstimatedDuration = "10 weeks",
                WorkoutsPerWeek = 5, ProgramType = "Dynamic Mobility + Strength"
            },

            // ---- Learn the Basic ----
            new FitnessPlanConfig
            {
                PlanId = "learn-the-basic-weak", PlanName = "Beginner Basics",
                Description = "Introductory plan teaching fundamentals for low calorie targets.",
                Goal = FitnessGoal.LearnTheBasic, Status = PlanStatus.Weak,
                MinCalorie = 0, MaxCalorie = 1800, EstimatedDuration = "6 weeks",
                WorkoutsPerWeek = 2, ProgramType = "Fundamentals"
            },
            new FitnessPlanConfig
            {
                PlanId = "learn-the-basic-normal", PlanName = "Core Basics",
                Description = "Standard fundamentals plan for normal calorie targets.",
                Goal = FitnessGoal.LearnTheBasic, Status = PlanStatus.Normal,
                MinCalorie = 1801, MaxCalorie = 2500, EstimatedDuration = "8 weeks",
                WorkoutsPerWeek = 3, ProgramType = "Fundamentals + Cardio"
            },
            new FitnessPlanConfig
            {
                PlanId = "learn-the-basic-hard", PlanName = "Applied Basics",
                Description = "Fundamentals plan with added intensity for high calorie targets.",
                Goal = FitnessGoal.LearnTheBasic, Status = PlanStatus.Hard,
                MinCalorie = 2501, MaxCalorie = double.MaxValue, EstimatedDuration = "8 weeks",
                WorkoutsPerWeek = 4, ProgramType = "Fundamentals + Strength"
            },
        };
        }
}
