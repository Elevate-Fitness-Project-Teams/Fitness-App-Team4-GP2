namespace ProgressService.Features.ViewProgressDashboard.DTOs
{
    public record WorkoutHistoryDto(int Id , int DurationInMinutes , int CaloriesBurned, DateTime CompletedAt);
    
}
