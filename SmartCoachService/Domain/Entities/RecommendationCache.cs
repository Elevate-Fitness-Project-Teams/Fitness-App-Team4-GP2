using System;
namespace SmartCoachService.Domain.Entities
{
    public class RecommendationCache
    {
        public Guid UserId { get; set; }
        public string UserContextJson { get; set; } = null!;
        public string? HomeFeedDataJson { get; set; }
        public DateTime CachedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
