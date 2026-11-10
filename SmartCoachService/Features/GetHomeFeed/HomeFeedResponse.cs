namespace SmartCoachService.Features.GetHomeFeed
{
    /// <summary>
    /// The aggregated Home Feed payload returned by GET /api/v1/home and persisted
    /// as RecommendationCache.HomeFeedDataJson (cache-first edge-aggregator pattern).
    ///
    /// A section is null when that service answered normally but has nothing for this user
    /// yet (new account, metrics not calculated, empty catalog). Null means "not available
    /// yet", which is deliberately distinct from a zeroed section - the feed must never
    /// report a fabricated 0 kcal calorie target as if it were real. If a service is
    /// genuinely down the whole endpoint returns 503 instead.
    /// </summary>
    public record HomeFeedResponse(
        HomeFeedProfileSection? Profile,
        HomeFeedMetricsSection? Metrics,
        IReadOnlyList<HomeFeedWorkoutItem> SuggestedWorkouts,
        HomeFeedNutritionSection? Nutrition,
        HomeFeedProgressSection? Progress,
        DateTime GeneratedAt);

    // Sourced from the Profile service (GET /api/v1/profile).
    public record HomeFeedProfileSection(
        string DisplayName,
        string? ProfilePictureUrl,
        double? Height,
        double? Weight);

    // Sourced from the FCE (Fitness Calculation Engine) service.
    public record HomeFeedMetricsSection(
        double Bmr,
        double Tdee,
        double CalorieTarget);

    // Sourced from the Workout service (GET /api/v1/WorkOut/GetWorkOuts).
    public record HomeFeedWorkoutItem(
        int WorkoutId,
        string Name,
        string Category,
        int DurationInMinutes,
        string Difficulty);

    // Sourced from the Nutrition service (GET /api/v1/nutrition/recommendations/{userId}).
    public record HomeFeedNutritionSection(
        double DailyGoalCalories,
        IReadOnlyList<HomeFeedMealItem> RecommendedMeals);

    public record HomeFeedMealItem(
        int MealId,
        string Name,
        string Type,
        double Calories,
        double Protein);

    // Sourced from the Progress service.
    public record HomeFeedProgressSection(
        int TotalWorkouts,
        int TotalCaloriesBurned,
        double TotalWeightLost,
        int CurrentStreak,
        int LongestStreak);
}
