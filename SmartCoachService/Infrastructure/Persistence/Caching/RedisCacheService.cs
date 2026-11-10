using Microsoft.Extensions.Caching.Distributed;
using SmartCoachService.Domain.Contracts;
using System.Text.Json;

namespace SmartCoachService.Infrastructure.Persistence.Caching
{
    public class RedisCacheService : ICacheService
    {
        private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

        private readonly IDistributedCache _cache;
        private readonly ILogger<RedisCacheService> _logger;

        public RedisCacheService(IDistributedCache cache, ILogger<RedisCacheService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            try
            {
                var json = await _cache.GetStringAsync(key, cancellationToken);
                return string.IsNullOrEmpty(json)
                    ? default
                    : JsonSerializer.Deserialize<T>(json, SerializerOptions);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogWarning(ex, "Cache read failed for key {CacheKey}; treating as a miss.", key);
                return default;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken cancellationToken = default)
        {
            try
            {
                var json = JsonSerializer.Serialize(value, SerializerOptions);
                var options = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = ttl };
                await _cache.SetStringAsync(key, json, options, cancellationToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogWarning(ex, "Cache write failed for key {CacheKey}; continuing without caching.", key);
            }
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            try
            {
                await _cache.RemoveAsync(key, cancellationToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogWarning(ex, "Cache eviction failed for key {CacheKey}.", key);
            }
        }
    }
}
