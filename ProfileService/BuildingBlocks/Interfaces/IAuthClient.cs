using BuildingBlocks.Shared.Results;

namespace ProfileService.BuildingBlocks.Interfaces
{
    public interface IAuthClient
    {
        Task<Result<bool>> UpdateEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}
