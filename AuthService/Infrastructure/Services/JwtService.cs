using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateAccessToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email ?? string.Empty),
                new(ClaimTypes.Name, user.UserName ?? string.Empty),
                new("firstName", user.FirstName),
                new("lastName", user.LastName),
                new("isPremium", user.IsPremium.ToString())
            };

            var minutes = _configuration.GetValue<int?>("Jwt:AccessTokenMinutes") ?? 60;
            return GenerateToken(claims, minutes);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public string GenerateResetToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email ?? string.Empty),
                new("purpose", "password_reset"),
                // Binds the token to the user's current security stamp. A successful reset
                // rotates the stamp, so the token can only be consumed once.
                new("stamp", user.SecurityStamp ?? string.Empty)
            };

            var minutes = _configuration.GetValue<int?>("Jwt:ResetTokenMinutes") ?? 15;
            return GenerateToken(claims, minutes);
        }

        public ClaimsPrincipal? ValidateResetToken(string token)
        {
            var key = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(key))
                return null;

            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = new JwtSecurityTokenHandler()
                    .ValidateToken(token, parameters, out _);

                // Reject access tokens or anything not explicitly issued for password reset.
                if (principal.FindFirst("purpose")?.Value != "password_reset")
                    return null;

                return principal;
            }
            catch
            {
                return null;
            }
        }

        private string GenerateToken(IEnumerable<Claim> claims, int expiryMinutes)
        {
            var key = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("Jwt:Key is not configured.");

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
