using BuildingBlocks.Shared.Results;
using MediatR;
using ProfileService.Features.UpdateProfile.Dtos;

namespace ProfileService.Features.UpdateProfile
{
    public record UpdateProfileCommand(UpdateProfileRequest Dto) : IRequest<Result<UpdateProfileResponse>>;
}
