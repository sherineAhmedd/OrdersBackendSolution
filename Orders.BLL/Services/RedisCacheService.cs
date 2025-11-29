
using Microsoft.Extensions.Logging;
using Orders.BLL.ServiceInterfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Orders.BLL.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _db;
        private readonly ILogger<RedisCacheService> _logger;
        public RedisCacheService(IConnectionMultiplexer redis , ILogger<RedisCacheService> logger)
        {
            _db = redis.GetDatabase();
            _logger = logger;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                var value = await _db.StringGetAsync(key);
                if (value.IsNullOrEmpty) return default;
                return JsonSerializer.Deserialize<T>((string)value);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to get cache for key {Key}", key);
                return default;
            }
            
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                await _db.KeyDeleteAsync(key);
                _logger.LogInformation("Cache removed for key {Key}", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to remove cache for key {Key}", key);
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan ttl)
        {
            try
            {
                var json = JsonSerializer.Serialize(value);
                await _db.StringSetAsync(key, json, ttl);
                _logger.LogInformation("Cache set for key {Key} with TTL {TTL}", key, ttl);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to set cache for key {Key}", key);
            }
            
        }
    }
}
