using BuildingBlocks.Shared.Results;

namespace FCEService.Common
{
    public static class FceErrors
    {
        
        public static Error RequiredField(string fieldName) =>
            Error.Validation("VAL_REQUIRED_FIELD", $"{fieldName} is required.");

        public static Error InvalidAge =>
            Error.Validation("VAL_INVALID_AGE", "Age must be between 16 and 100 years.");

        public static Error InvalidWeight =>
            Error.Validation("VAL_INVALID_WEIGHT", "Weight must be between 40 and 200 kg.");

        public static Error InvalidHeight =>
            Error.Validation("VAL_INVALID_HEIGHT", "Height must be between 140 and 220 cm.");

        public static Error InvalidGender =>
            Error.Validation("VAL_INVALID_GENDER", "Gender must be 'Male' or 'Female'.");

        public static Error InvalidGoal =>
            Error.Validation("VAL_INVALID_GOAL",
                "Goal must be one of: Lose Weight, Get Fitter, Gain Weight, Gain More Flexible, Learn the Basic.");

        public static Error InvalidActivity =>
            Error.Validation("VAL_INVALID_ACTIVITY",
                "ActivityLevel must be one of: Rookie, Beginner, Intermediate, Advance, TrueBeast.");

     
        public static Error StatsNotFound =>
            Error.NotFound("FCE_STATS_NOT_FOUND",
                "No fitness stats found. Please submit your stats via /weight-goal-activity first.");

        public static Error MetricsNotCalculated =>
            Error.Validation("FCE_METRICS_NOT_CALCULATED",
                "No metrics calculated yet. Please run /calculate first.");

        public static Error InvalidCalculation =>
            Error.Validation("FCE_INVALID_CALCULATION",
                "Calculation produced an invalid numeric result. Please verify your stats.");

        public static Error NoMatchingPlan(string goal, string status) =>
            Error.NotFound("FCE_NO_MATCHING_PLAN",
                $"No fitness plan found for Goal={goal} and Status={status}.");

        public static Error PlanAlreadyAssigned =>
            Error.Conflict("FCE_PLAN_ALREADY_ASSIGNED",
                "This plan is already the active plan for the user.");

      
        public static Error PlanNotFound(string planId) =>
            Error.NotFound("RES_PLAN_NOT_FOUND", $"Plan '{planId}' does not exist.");
    }
}
