using BuildingBlocks.Shared.Results;
using MediatR;
using SubscriptionService.Domain.Contracts.Repositories;

namespace SubscriptionService.Features.GetSubscriptionStatus
{
    public class GetSubscriptionStatusHandler : IRequestHandler<GetSubscriptionStatusQuery, Result<SubscriptionStatusResponse>>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public GetSubscriptionStatusHandler(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }
        public async Task<Result<SubscriptionStatusResponse>> Handle(GetSubscriptionStatusQuery request, CancellationToken cancellationToken)
        {

            var subscription =  await _subscriptionRepository.GetSubscriptionStatus(request.UserId);

            if (subscription is null)
                return  new SubscriptionStatusResponse("Free", "Active", null);

            return subscription;
        }

    }
}
