using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using BuildingBlocks.Shared.Responses;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace ApiGateway.RateLimiting
{
    public enum UserTier
    {
        Anonymous = 0,
        Free = 1,
        Premium = 2
    }

    public class TierRateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConnectionMultiplexer? _redis;
        private readonly ILogger<TierRateLimitingMiddleware> _logger;
        private static readonly ConcurrentDictionary<string, (int Count, long ResetTime)> _inMemoryStore = new();
        private static readonly JwtSecurityTokenHandler _tokenHandler = new();

        public TierRateLimitingMiddleware(
            RequestDelegate _next,
            ILogger<TierRateLimitingMiddleware> logger,
            IConnectionMultiplexer? redis = null)
        {
            this._next = _next;
            _logger = logger;
            _redis = redis;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var (tier, clientKey) = ResolveClientTierAndKey(context);
            var limit = GetTierHourlyLimit(tier);

            var currentWindow = DateTimeOffset.UtcNow.ToUnixTimeSeconds() / 3600;
            var resetTime = (currentWindow + 1) * 3600;

            var currentCount = await IncrementRequestCountAsync(clientKey, currentWindow, resetTime);
            var remaining = Math.Max(0, limit - currentCount);

            context.Response.Headers["X-RateLimit-Limit"] = limit.ToString();
            context.Response.Headers["X-RateLimit-Remaining"] = remaining.ToString();
            context.Response.Headers["X-RateLimit-Reset"] = resetTime.ToString();

            if (currentCount > limit)
            {
                _logger.LogWarning("Rate limit exceeded for key {Key} on tier {Tier}. Limit: {Limit}, Actual: {Count}",
                    clientKey, tier, limit, currentCount);

                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                context.Response.ContentType = "application/json";

                var failureResponse = ResponseFactory.Failure(
                    $"Rate limit exceeded. Tier limit is {limit} requests per hour.",
                    StatusCodes.Status429TooManyRequests,
                    "RATE_LIMIT_EXCEEDED");

                await context.Response.WriteAsync(JsonSerializer.Serialize(failureResponse));
                return;
            }

            await _next(context);
        }

        private (UserTier Tier, string Key) ResolveClientTierAndKey(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                var tokenString = authHeader.Substring("Bearer ".Length).Trim();
                try
                {
                    if (_tokenHandler.CanReadToken(tokenString))
                    {
                        var jwt = _tokenHandler.ReadJwtToken(tokenString);
                        var userIdClaim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub || c.Type == ClaimTypes.NameIdentifier);
                        if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
                        {
                            var isPremiumClaim = jwt.Claims.FirstOrDefault(c => c.Type.Equals("isPremium", StringComparison.OrdinalIgnoreCase))?.Value;
                            var isPremium = bool.TryParse(isPremiumClaim, out var p) && p;

                            return isPremium
                                ? (UserTier.Premium, $"user:{userId}")
                                : (UserTier.Free, $"user:{userId}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogDebug(ex, "Failed to parse JWT for rate limiting; falling back to anonymous tier.");
                }
            }

            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown_client";
            return (UserTier.Anonymous, $"ip:{ip}");
        }

        private static int GetTierHourlyLimit(UserTier tier) => tier switch
        {
            UserTier.Anonymous => 50,
            UserTier.Free => 500,
            UserTier.Premium => 2000,
            _ => 50
        };

        private async Task<int> IncrementRequestCountAsync(string clientKey, long windowId, long resetTime)
        {
            if (_redis != null && _redis.IsConnected)
            {
                try
                {
                    var db = _redis.GetDatabase();
                    var redisKey = $"ratelimit:{clientKey}:{windowId}";
                    var count = (int)await db.StringIncrementAsync(redisKey);
                    if (count == 1)
                    {
                        await db.KeyExpireAsync(redisKey, TimeSpan.FromHours(1.5));
                    }
                    return count;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Redis rate limiting error, falling back to in-memory store.");
                }
            }

            // In-Memory Fallback
            var memKey = $"{clientKey}:{windowId}";
            var entry = _inMemoryStore.AddOrUpdate(
                memKey,
                _ => (1, resetTime),
                (_, existing) => (existing.Count + 1, resetTime));

            // Clean expired memory keys periodically
            if (_inMemoryStore.Count > 10000)
            {
                var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                foreach (var k in _inMemoryStore.Keys)
                {
                    if (_inMemoryStore.TryGetValue(k, out var val) && val.ResetTime < now)
                        _inMemoryStore.TryRemove(k, out _);
                }
            }

            return entry.Count;
        }
    }
}

