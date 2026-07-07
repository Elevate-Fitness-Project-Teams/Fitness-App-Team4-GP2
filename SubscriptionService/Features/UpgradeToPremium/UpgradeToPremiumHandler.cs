using BuildingBlocks.Contracts.Subscription;
using BuildingBlocks.Shared.Results;
using MassTransit;
using MassTransit.Courier.Contracts;
using MediatR;
using SubscriptionService.Domain.Contracts;
using SubscriptionService.Domain.Contracts.Repositories;
using SubscriptionService.Domain.Entities;
using SubscriptionService.Features.GetSubscriptionStatus;

namespace SubscriptionService.Features.UpgradeToPremium
{


    public class UpgradeToPremiumHandler : IRequestHandler<UpgradeToPremiumCommand, Result<SubscriptionStatusResponse>>
    {
        
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IGenericRepository<UserSubscription> _subscriptionRepository;
        private readonly IGenericRepository<BillingLog> _billingLogRepository;
        private readonly IBillingService _billingService;

        public UpgradeToPremiumHandler(IGenericRepository<UserSubscription> subscriptionRepository
            , IGenericRepository<BillingLog> billingLogRepository
            , IPublishEndpoint publishEndpoint
            ,IBillingService billingService)
        { 
             _publishEndpoint = publishEndpoint;
            _subscriptionRepository = subscriptionRepository;
            _billingLogRepository = billingLogRepository;
            _billingService = billingService;
        }

        public async Task<Result<SubscriptionStatusResponse>> Handle(UpgradeToPremiumCommand request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var BillingAuth = await _billingService.AuthorizeAsync(userId , request.Tier , request.DurationMonths);
            if (!BillingAuth.IsSuccess)
                return Error.Failure("Failure", "SRV_DATABASE_ERROR");
            
            try
            {
                var now = DateTime.UtcNow;
                var tier = request.Tier;
                var expiresAt = now.AddMonths(request.DurationMonths);
                var status = "Active";
                var billingLog = new BillingLog
                {
                    UserId = userId,
                    ReferenceId = BillingAuth.ReferenceId,
                    Amount = BillingAuth.Amount,
                    Currency = BillingAuth.Currency,
                    PaymentStatus = "Success",
                    Timestamp = DateTime.UtcNow
                };

                _billingLogRepository.Add(billingLog);

                var subscription = await _subscriptionRepository.GetOneAsync(x=>x.UserId == userId);
                
                if(subscription is not null)
                {
                    subscription.Tier = request.Tier;
                    subscription.Status = status;
                    subscription.ExpiresAt = expiresAt;
                    subscription.UpdatedAt = now;


                }
                else
                {
                    subscription = new UserSubscription
                    {
                        UserId = userId,
                        Tier = request.Tier,
                        Status = status,
                        StartsAt = now,
                        UpdatedAt = now,
                        ExpiresAt = expiresAt
                    };
                    _subscriptionRepository.Add(subscription);
                }
                await _subscriptionRepository.SaveChangesAsync();

                await _publishEndpoint.Publish(new SubscriptionUpgradedEvent
                {
                    UserId = userId,
                    Plan = tier.ToString(),
                    IsPremium = true,
                    UpgradedAt = now
                });

                return new SubscriptionStatusResponse(tier.ToString(), status, expiresAt, now);

            }
            catch (Exception e)
            {
                throw new Exception($"Error upgrading user {request.UserId} to premium: {e.Message}", e);
            }
        }
    }
}
