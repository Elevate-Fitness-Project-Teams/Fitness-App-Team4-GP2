using SubscriptionService.Domain.Contracts;
using SubscriptionService.Domain.Entities;

namespace SubscriptionService.Infrastructure.Services
{
    public class BillingService : IBillingService
    {


        public Task<BillingResult> AuthorizeAsync(Guid userId , SubscriptionTier planTier , int durationMonths)
        {
            var random = new Random();
            
            var isSuccess = Task.FromResult(
                random.Next(1, 101) <= 80
            );
            var amount = planTier switch
            {
                SubscriptionTier.Premium => 199.99m * durationMonths,
                _ => 0m
            };

            return Task.FromResult(new BillingResult(
                IsSuccess: isSuccess.Result,
                Amount: amount,
                ReferenceId: Guid.NewGuid().ToString(),
                Currency: "EGP"
            ));
        }
    }
}

