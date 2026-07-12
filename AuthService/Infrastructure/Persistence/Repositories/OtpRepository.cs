using AuthService.BuildingBlocks.Interfaces;
using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace AuthService.Infrastructure.Persistence.Repositories
{
    public class OtpRepository : GenericRepository<OtpCode>, IOtpRepository
    {

        public OtpRepository(AuthDbContext context) : base(context)
        {
        }

        // Cryptographically secure 6-digit code (100000-999999). GetInt32's upper bound is
        // exclusive, so 1_000_000 yields an inclusive 999999.
        public string Generate() => RandomNumberGenerator.GetInt32(100_000, 1_000_000).ToString();

        public Task<OtpCode?> GetLatestOtpAsync(string email) =>
            _context.OtpCodes
            .Where(x => x.Email == email)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();

        public string Hash(string otp) => BCrypt.Net.BCrypt.HashPassword(otp);

        public bool Verify(string otp, string hash) => BCrypt.Net.BCrypt.Verify(otp, hash);
    }
}
