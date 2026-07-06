using BuildingBlocks.Shared.Results;
using MediatR;
using ProfileService.Features.ViewProfile.Dtos;

namespace ProfileService.Features.ViewProfile
{
    public record ViewProfileQuery : IRequest<Result<ViewProfileResponse>>;
}
