using System;
namespace SmartCoachService.Domain.Entities
{
    public class RecommendationCache
    {
        public int UserId { get; set; }
        public string UserContextJson { get; set; } = null!;
        public string HomeFeedDataJson { get; set; } = null!;
        public DateTime CachedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
