using System;
namespace Elevate.FitnessApp.Features.Auth.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        // Navigation property
        public User User { get; set; } = null!;
    }
}
