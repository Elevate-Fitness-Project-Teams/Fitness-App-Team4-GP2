using AuthService.BuildingBlocks.Interfaces;
using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using System.Threading;

namespace AuthService.Infrastructure.Persistence.Repositories
{
    public class OtpRepository : GenericRepository<OtpCode>, IOtpRepository
    {

        public OtpRepository(AuthDbContext context) : base(context)
        {
        }

        public string Generate() => Random.Shared.Next(100000, 999999).ToString();

        public Task<OtpCode?> GetLatestOtpAsync(string email) =>
            _context.OtpCodes
            .Where(x => x.Email == email)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();

        public string Hash(string otp) => BCrypt.Net.BCrypt.HashPassword(otp);

        public bool Verify(string otp, string hash) => BCrypt.Net.BCrypt.Verify(otp, hash);
    }
}
