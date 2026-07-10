using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FCEService.Migrations
{
    /// <inheritdoc />
    public partial class SeedFitnessPlanConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FitnessPlanConfigs",
                columns: new[] { "PlanId", "Description", "EstimatedDuration", "Goal", "MaxCalorie", "MinCalorie", "PlanName", "ProgramType", "Status", "WorkoutsPerWeek" },
                values: new object[,]
                {
                    { "gain-more-flexible-hard", "Advanced flexibility/mobility plan for high calorie targets.", "10 weeks", "GainMoreFlexible", 1.7976931348623157E+308, 2501.0, "Advanced Mobility", "Dynamic Mobility + Strength", "Hard", 5 },
                    { "gain-more-flexible-normal", "Balanced flexibility/mobility plan for normal calorie targets.", "8 weeks", "GainMoreFlexible", 2500.0, 1801.0, "Flexibility & Mobility", "Yoga + Mobility", "Normal", 4 },
                    { "gain-more-flexible-weak", "Entry-level flexibility/mobility plan for low calorie targets.", "6 weeks", "GainMoreFlexible", 1800.0, 0.0, "Mobility Starter", "Mobility + Stretching", "Weak", 3 },
                    { "gain-weight-hard", "Advanced muscle gain plan for high calorie targets.", "12 weeks", "GainWeight", 1.7976931348623157E+308, 2501.0, "Mass Gain", "Heavy Strength + Volume", "Hard", 5 },
                    { "gain-weight-normal", "Balanced muscle gain plan for normal calorie targets.", "12 weeks", "GainWeight", 2500.0, 1801.0, "Lean Bulk", "Hypertrophy Strength", "Normal", 4 },
                    { "gain-weight-weak", "Entry-level muscle gain plan for low calorie targets.", "10 weeks", "GainWeight", 1800.0, 0.0, "Lean Bulk Starter", "Strength Foundations", "Weak", 3 },
                    { "get-fitter-hard", "Advanced general fitness plan for high calorie targets.", "10 weeks", "GetFitter", 1.7976931348623157E+308, 2501.0, "Advanced Fitness", "Full-Body + HIIT", "Hard", 5 },
                    { "get-fitter-normal", "Balanced general fitness plan for normal calorie targets.", "8 weeks", "GetFitter", 2500.0, 1801.0, "General Fitness", "Full-Body + Cardio", "Normal", 4 },
                    { "get-fitter-weak", "Entry-level general fitness plan for low calorie targets.", "6 weeks", "GetFitter", 1800.0, 0.0, "Foundation Fitness", "Full-Body Basics", "Weak", 3 },
                    { "learn-the-basic-hard", "Fundamentals plan with added intensity for high calorie targets.", "8 weeks", "LearnTheBasic", 1.7976931348623157E+308, 2501.0, "Applied Basics", "Fundamentals + Strength", "Hard", 4 },
                    { "learn-the-basic-normal", "Standard fundamentals plan for normal calorie targets.", "8 weeks", "LearnTheBasic", 2500.0, 1801.0, "Core Basics", "Fundamentals + Cardio", "Normal", 3 },
                    { "learn-the-basic-weak", "Introductory plan teaching fundamentals for low calorie targets.", "6 weeks", "LearnTheBasic", 1800.0, 0.0, "Beginner Basics", "Fundamentals", "Weak", 2 },
                    { "lose-weight-hard", "High-intensity plan for users with a high calorie target aiming to lose weight.", "10 weeks", "LoseWeight", 1.7976931348623157E+308, 2501.0, "Intensive Fat Loss", "HIIT + Strength", "Hard", 5 },
                    { "lose-weight-normal", "Moderate-intensity plan for users with a normal calorie target aiming to lose weight.", "8 weeks", "LoseWeight", 2500.0, 1801.0, "Balanced Fat Loss", "Cardio + Strength", "Normal", 4 },
                    { "lose-weight-weak", "Low-intensity plan for users with low calorie targets aiming to lose weight.", "8 weeks", "LoseWeight", 1800.0, 0.0, "Gentle Fat Loss", "Low-Impact Cardio", "Weak", 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FitnessPlanConfigs",
                keyColumn: "PlanId",
                keyValue: "gain-more-flexible-hard");

            migrationBuilder.DeleteData(
                table: "FitnessPlanConfigs",
                keyColumn: "PlanId",
                keyValue: "gain-more-flexible-normal");

            migrationBuilder.DeleteData(
                table: "FitnessPlanConfigs",
                keyColumn: "PlanId",
                keyValue: "gain-more-flexible-weak");

            migrationBuilder.DeleteData(
                table: "FitnessPlanConfigs",
                keyColumn: "PlanId",
                keyValue: "gain-weight-hard");

            migrationBuilder.DeleteData(
                table: "FitnessPlanConfigs",
                keyColumn: "PlanId",
                keyValue: "gain-weight-normal");

            migrationBuilder.DeleteData(
                table: "FitnessPlanConfigs",
                keyColumn: "PlanId",
                keyValue: "gain-weight-weak");

            migrationBuilder.DeleteData(
                table: "FitnessPlanConfigs",
                keyColumn: "PlanId",
                keyValue: "get-fitter-hard");

            migrationBuilder.DeleteData(
                table: "FitnessPlanConfigs",
                keyColumn: "PlanId",
                keyValue: "get-fitter-normal");

            migrationBuilder.DeleteData(
                table: "FitnessPlanConfigs",
                keyColumn: "PlanId",
                keyValue: "get-fitter-weak");

            migrationBuilder.DeleteData(
                table: "FitnessPlanConfigs",
                keyColumn: "PlanId",
                keyValue: "learn-the-basic-hard");

            migrationBuilder.DeleteData(
                table: "FitnessPlanConfigs",
                keyColumn: "PlanId",
                keyValue: "learn-the-basic-normal");

            migrationBuilder.DeleteData(
                table: "FitnessPlanConfigs",
                keyColumn: "PlanId",
                keyValue: "learn-the-basic-weak");

            migrationBuilder.DeleteData(
                table: "FitnessPlanConfigs",
                keyColumn: "PlanId",
                keyValue: "lose-weight-hard");

            migrationBuilder.DeleteData(
                table: "FitnessPlanConfigs",
                keyColumn: "PlanId",
                keyValue: "lose-weight-normal");

            migrationBuilder.DeleteData(
                table: "FitnessPlanConfigs",
                keyColumn: "PlanId",
                keyValue: "lose-weight-weak");
        }
    }
}
