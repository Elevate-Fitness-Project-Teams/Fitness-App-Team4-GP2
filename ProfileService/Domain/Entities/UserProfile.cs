using System;
namespace ProfileService.Domain.Entities
{
    public class UserProfile
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? ProfilePictureUrl { get; set; }
        public bool IsPremiumCached { get; set; }
        public DateTime MemberSince { get; set; }
    }
}