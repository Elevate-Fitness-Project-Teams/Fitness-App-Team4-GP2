using System;
namespace Elevate.FitnessApp.Features.Auth.Entities
{
    public class OtpCode
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Code { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; }
    }
}