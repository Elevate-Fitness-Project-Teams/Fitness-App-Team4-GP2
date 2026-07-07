using BuildingBlocks.Shared.Results;
using MediatR;
using SubscriptionService.Domain.Contracts.Repositories;
using SubscriptionService.Domain.Entities;

namespace SubscriptionService.Features.CancelSubscription
{
    public class CancelSubscriptionHandler : IRequestHandler<CancelSubscriptionCommand, Result<bool>>
    {
        private readonly IGenericRepository<UserSubscription> _subscriptionRepository;

        public CancelSubscriptionHandler(IGenericRepository<UserSubscription> subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }
        public async Task<Result<bool>> Handle(CancelSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var subscription = await _subscriptionRepository.GetOneAsync(x => x.UserId == request.UserId && x.Status == "Active");
            if (subscription is null)
                return Error.NotFound("NotFound", "RES_NOT_FOUND");

            if (!subscription.AutoRenew)
                return Error.Conflict("Conflict", "RES_ALREADY_CANCELED");

            subscription.AutoRenew = false;
            await _subscriptionRepository.SaveChangesAsync();

            return true;
        }
    }
}
