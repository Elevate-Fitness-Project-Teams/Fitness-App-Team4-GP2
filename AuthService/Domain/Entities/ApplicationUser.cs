using Microsoft.AspNetCore.Identity;
using System;
namespace AuthService.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool RequiresProfileCompletion { get; set; }
        public bool IsPremium { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
