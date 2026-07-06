namespace ProfileService.Features.ViewProfile.Dtos
{
    public class ProgressStatisticsDto
    {
        public int TotalWorkouts { get; set; }
        public int CurrentStreak { get; set; }
        public int TotalCaloriesBurned { get; set; }
        public double TotalWeightLost { get; set; }
        public int LongestStreak { get; set; }

    }
}
