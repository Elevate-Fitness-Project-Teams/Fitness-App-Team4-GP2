using BuildingBlocks.Shared.Results;
using MediatR;

namespace AuthService.Features.Profile.UpdateUserInfo
{
    public record UpdateUserInfoCommand(string FirstName, string LastName, string PhoneNumber, string Email)
        : IRequest<Result<bool>>;
}
