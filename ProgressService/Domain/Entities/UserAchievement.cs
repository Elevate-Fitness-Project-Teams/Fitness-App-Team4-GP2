using System;
namespace ProgressService.Domain.Entities
{
    public class UserAchievement : BaseEntity
    {
        public string UserId { get; set; } = null!;
        public int AchievementId { get; set; }
        public DateTime EarnedAt { get; set; }
        // Navigation properties
        public Achievement Achievement { get; set; } = null!;
    }
}
