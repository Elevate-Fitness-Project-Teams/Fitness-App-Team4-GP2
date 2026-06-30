namespace ProgressService.Features.ViewProgressDashboard.DTOs
{
    public record StatisticsSummaryDto(int TotalWorkouts , int TotalCaloriesBurned , double TotalWeightLost, DateTime UpdatedAt);
    
}
