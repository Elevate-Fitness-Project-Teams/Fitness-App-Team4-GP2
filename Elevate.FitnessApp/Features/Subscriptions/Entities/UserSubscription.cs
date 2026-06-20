using System;
using Elevate.FitnessApp.Features.Auth.Entities;
namespace Elevate.FitnessApp.Features.Subscriptions.Entities
{
    public class UserSubscription
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Tier { get; set; } = "Free";
        public string Status { get; set; } = "Active";
        public DateTime StartsAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        // Navigation property
        public User User { get; set; } = null!;
    }
}