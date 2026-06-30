using AuthService.Domain.Entities;

namespace AuthService.BuildingBlocks.Interfaces
{
    public interface IOtpRepository
    {
        Task<OtpCode?> GetLatestOtpAsync(string email);

        Task AddAsync(OtpCode otp);
    }
}
