namespace SubscriptionService.Domain.Entities
{
    public class BillingLog
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string ReferenceId { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "EGP";
        public string PaymentStatus { get; set; } = "Success";
        public DateTime Timestamp { get; set; }
    }
}
