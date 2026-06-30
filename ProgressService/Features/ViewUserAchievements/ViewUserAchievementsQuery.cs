using BuildingBlocks.Shared.Results;
using MediatR;

namespace ProgressService.Features.ViewUserAchievements
{
    public record ViewUserAchievementsQuery(Guid UserId) : IRequest<Result<UserAchievementsResponse>>;

}
