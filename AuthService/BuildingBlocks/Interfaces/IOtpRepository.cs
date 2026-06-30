using AuthService.Domain.Entities;

namespace AuthService.BuildingBlocks.Interfaces
{
    public interface IOtpRepository : IGenericRepository<OtpCode>
    {
        Task<OtpCode?> GetLatestOtpAsync(string email);
        string Generate();
        string Hash(string otp);
        bool Verify(string otp, string hash);
    }
}
