using BuildingBlocks.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SubscriptionService.Domain.Contracts.Repositories;
using SubscriptionService.Domain.Entities;

namespace SubscriptionService.Features.GetSubscriptionStatus
{
    public class GetSubscriptionStatusHandler : IRequestHandler<GetSubscriptionStatusQuery, Result<SubscriptionStatusResponse>>
    {
        private readonly IGenericRepository<UserSubscription> _subscriptionRepository;

        public GetSubscriptionStatusHandler(IGenericRepository<UserSubscription> subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }
        public async Task<Result<SubscriptionStatusResponse>> Handle(GetSubscriptionStatusQuery request, CancellationToken cancellationToken)
        {

            var subscription = await _subscriptionRepository.GetAll(x=> x.UserId == request.UserId)
                .Select(x=> new SubscriptionStatusResponse
                (x.Tier.ToString(),
                x.Status,
                x.ExpiresAt,
                x.UpdatedAt)
                ).FirstOrDefaultAsync();

            if (subscription is null)
                return  new SubscriptionStatusResponse("Free", "Active", null , null);

            return subscription;
        }

    }
}
