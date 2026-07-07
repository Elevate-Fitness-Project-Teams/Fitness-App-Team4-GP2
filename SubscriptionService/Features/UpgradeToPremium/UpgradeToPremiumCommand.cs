using BuildingBlocks.Shared.Results;
using MediatR;
using SubscriptionService.Domain.Entities;
using SubscriptionService.Features.GetSubscriptionStatus;

namespace SubscriptionService.Features.UpgradeToPremium
{
    public record UpgradeToPremiumCommand(Guid UserId , SubscriptionTier Tier , int DurationMonths) : IRequest<Result<SubscriptionStatusResponse>>;

    
}
