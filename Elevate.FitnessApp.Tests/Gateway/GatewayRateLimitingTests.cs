using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiGateway.RateLimiting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Elevate.FitnessApp.Tests.Gateway
{
    public class GatewayRateLimitingTests
    {
        private static string GenerateTestJwt(Guid userId, bool isPremium)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("SuperSecretLongKeyForElevateFitnessAppTestingOnly12345!");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                    new Claim("isPremium", isPremium.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        [Fact]
        public async Task Anonymous_User_Receives_Limit_Of_50_And_RateLimit_Headers()
        {
            var context = new DefaultHttpContext();
            context.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("192.168.1.100");

            var wasCalled = false;
            RequestDelegate next = (ctx) =>
            {
                wasCalled = true;
                return Task.CompletedTask;
            };

            var middleware = new TierRateLimitingMiddleware(next, NullLogger<TierRateLimitingMiddleware>.Instance);

            await middleware.InvokeAsync(context);

            Assert.True(wasCalled);
            Assert.Equal("50", context.Response.Headers["X-RateLimit-Limit"].ToString());
            Assert.NotEmpty(context.Response.Headers["X-RateLimit-Remaining"].ToString());
            Assert.NotEmpty(context.Response.Headers["X-RateLimit-Reset"].ToString());
        }

        [Fact]
        public async Task Free_User_Receives_Limit_Of_500()
        {
            var userId = Guid.NewGuid();
            var token = GenerateTestJwt(userId, isPremium: false);

            var context = new DefaultHttpContext();
            context.Request.Headers["Authorization"] = $"Bearer {token}";

            var wasCalled = false;
            RequestDelegate next = (ctx) =>
            {
                wasCalled = true;
                return Task.CompletedTask;
            };

            var middleware = new TierRateLimitingMiddleware(next, NullLogger<TierRateLimitingMiddleware>.Instance);

            await middleware.InvokeAsync(context);

            Assert.True(wasCalled);
            Assert.Equal("500", context.Response.Headers["X-RateLimit-Limit"].ToString());
        }

        [Fact]
        public async Task Premium_User_Receives_Limit_Of_2000()
        {
            var userId = Guid.NewGuid();
            var token = GenerateTestJwt(userId, isPremium: true);

            var context = new DefaultHttpContext();
            context.Request.Headers["Authorization"] = $"Bearer {token}";

            var wasCalled = false;
            RequestDelegate next = (ctx) =>
            {
                wasCalled = true;
                return Task.CompletedTask;
            };

            var middleware = new TierRateLimitingMiddleware(next, NullLogger<TierRateLimitingMiddleware>.Instance);

            await middleware.InvokeAsync(context);

            Assert.True(wasCalled);
            Assert.Equal("2000", context.Response.Headers["X-RateLimit-Limit"].ToString());
        }

        [Fact]
        public async Task Exceeding_Rate_Limit_Returns_429_And_Blocks_Next_Middleware()
        {
            var context = new DefaultHttpContext();
            context.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("10.0.0.99");
            context.Response.Body = new MemoryStream();

            var wasCalled = false;
            RequestDelegate next = (ctx) =>
            {
                wasCalled = true;
                return Task.CompletedTask;
            };

            var middleware = new TierRateLimitingMiddleware(next, NullLogger<TierRateLimitingMiddleware>.Instance);

            // Anonymous limit is 50. Make 50 requests.
            for (int i = 0; i < 50; i++)
            {
                var reqContext = new DefaultHttpContext();
                reqContext.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("10.0.0.99");
                await middleware.InvokeAsync(reqContext);
            }

            // Reset flag before the 51st request
            wasCalled = false;

            // The 51st request must trigger 429 and block next
            await middleware.InvokeAsync(context);

            Assert.False(wasCalled, "Next middleware must not be invoked when rate limit is exceeded.");
            Assert.Equal(429, context.Response.StatusCode);
            Assert.Equal("0", context.Response.Headers["X-RateLimit-Remaining"].ToString());
        }
    }
}
