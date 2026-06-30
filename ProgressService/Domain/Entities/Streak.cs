using System;
namespace ProgressService.Domain.Entities
{
    public class Streak
    {
        public int UserId { get; set; }
        public int CurrentStreak { get; set; } = 0;
        public int LongestStreak { get; set; } = 0;
        public DateTime? LastWorkoutDate { get; set; }
    }
}
