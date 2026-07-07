using BuildingBlocks.Shared.Results;
using MediatR;

namespace SubscriptionService.Features.CancelSubscription
{
    public record CancelSubscriptionCommand(Guid UserId) : IRequest<Result<bool>>;
       
    
}
