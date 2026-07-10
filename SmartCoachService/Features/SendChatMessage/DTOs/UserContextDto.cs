namespace SmartCoachService.Features.SendChatMessage.DTOs
{
    public class UserContextDto
    {
        public double Bmr { get; set; }

        public double Tdee { get; set; }

        public double CalorieTarget { get; set; }

        public int TotalWorkouts { get; set; }

        public int TotalCaloriesBurned { get; set; }

        public double TotalWeightLost { get; set; }

        public int CurrentStreak { get; set; }

        public int LongestStreak { get; set; }
    }
}
