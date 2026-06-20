using System;
using Elevate.FitnessApp.Features.Auth.Entities;
namespace Elevate.FitnessApp.Features.Progress.Entities
{
    public class UserAchievement
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AchievementId { get; set; }
        public DateTime EarnedAt { get; set; }
        // Navigation properties
        public User User { get; set; } = null!;
        public Achievement Achievement { get; set; } = null!;
    }
}
