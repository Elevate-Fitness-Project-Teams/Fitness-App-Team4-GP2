using System;
namespace ProgressService.Domain.Entities
{
    public class UserAchievement : BaseEntity
    {
        public Guid UserId { get; set; }
        public int AchievementId { get; set; }
        public DateTime EarnedAt { get; set; }
        // Navigation properties
        public Achievement Achievement { get; set; } = null!;
    }
}
