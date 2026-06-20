using System;
using Elevate.FitnessApp.Features.Auth.Entities;
namespace Elevate.FitnessApp.Features.Profiles.Entities
{
    public class UserProfile
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? ProfilePictureUrl { get; set; }
        public bool IsPremiumCached { get; set; }
        public DateTime MemberSince { get; set; }
        // Navigation property to User (1-to-1 relation)
        public User User { get; set; } = null!;
    }
}