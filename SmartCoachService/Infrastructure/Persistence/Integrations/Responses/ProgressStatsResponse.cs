namespace SmartCoachService.Infrastructure.Persistence.Integrations.Responses
{
    public record ProgressStatsResponse(
    int TotalWorkouts,
    int TotalCaloriesBurned,
    double TotalWeightLost,
    int CurrentStreak,
    int LongestStreak
);


}
