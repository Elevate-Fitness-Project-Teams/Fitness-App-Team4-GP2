using BuildingBlocks.Shared.Results;
using MediatR;

namespace SmartCoachService.Features.GetHomeFeed
{
    public record GetHomeFeedQuery(Guid UserId) : IRequest<Result<HomeFeedResponse>>;
}
