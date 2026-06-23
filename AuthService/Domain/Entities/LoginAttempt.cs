using System;

namespace AuthService.Domain.Entities
{
    public class LoginAttempt
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public DateTime AttemptedAt { get; set; }
        public bool IsSuccess { get; set; }
        public string IpAddress { get; set; } = null!;
    }
}
