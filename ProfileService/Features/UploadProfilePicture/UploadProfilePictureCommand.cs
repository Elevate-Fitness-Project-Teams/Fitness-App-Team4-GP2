using BuildingBlocks.Shared.Results;
using MediatR;
using ProfileService.Features.UploadProfilePicture.Dto;

namespace ProfileService.Features.UploadProfilePicture
{
    public record UploadProfilePictureCommand(IFormFile ProfilePicture) : IRequest<Result<UploadProfilePictureResponse>>;
}
