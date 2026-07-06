using BuildingBlocks.Shared.Results;
using MediatR;

namespace SubscriptionService.Features.GetSubscriptionStatus
{
    public record GetSubscriptionStatusQuery(Guid UserId) : IRequest<Result<SubscriptionStatusResponse>>;
    
}
