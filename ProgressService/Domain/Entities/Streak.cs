using System;
namespace ProgressService.Domain.Entities
{
    public class Streak : BaseEntity
    {
        public string UserId { get; set; } = null!;
        public int CurrentStreak { get; set; } = 0;
        public int LongestStreak { get; set; } = 0;
        public DateTime? LastWorkoutDate { get; set; }
    }
}
