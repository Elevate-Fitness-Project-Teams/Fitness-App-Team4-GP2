using ProfileService.Features.ChangePassword.Dtos;
using BuildingBlocks.Shared.Results;

namespace ProfileService.BuildingBlocks.Interfaces
{
    public interface IAuthClient
    {
        Task<Result<bool>> UpdateUserInfoAsync(string firstName, string lastName, string phoneNumber, string email, CancellationToken cancellationToken = default);
        Task<Result<bool>> ChangePasswordAsync(ChangePasswordRequest request);
    }
}
