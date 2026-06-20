using System;
using Elevate.FitnessApp.Features.Auth.Entities;
namespace Elevate.FitnessApp.Features.Subscriptions.Entities
{
    public class BillingLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ReferenceId { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "EGP";
        public string PaymentStatus { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        // Navigation property
        public User User { get; set; } = null!;
    }
}
