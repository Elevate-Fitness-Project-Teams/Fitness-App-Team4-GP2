namespace SubscriptionService.Infrastructure.Services
{
    public record BillingResult(
    bool IsSuccess,
    decimal Amount,
    string ReferenceId,
    string Currency);
}
