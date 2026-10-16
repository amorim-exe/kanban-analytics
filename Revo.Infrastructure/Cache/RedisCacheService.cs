using StackExchange.Redis;
using System.Text.Json;

namespace Revo.Infrastructure.Cache;

public class RedisCacheService : ICacheService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _database;

    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _database = redis.GetDatabase();
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _database.StringGetAsync(key);
        if (value.IsNullOrEmpty) return default;

        return JsonSerializer.Deserialize<T>(value.ToString());
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var serialized = JsonSerializer.Serialize(value);
        if (expiry.HasValue)
        {
            await _database.StringSetAsync(key, serialized, expiry.Value);
        }
        else
        {
            await _database.StringSetAsync(key, serialized);
        }
    }

    public async Task RemoveAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
    }

    public async Task RemoveByPatternAsync(string pattern)
    {
        var server = _redis.GetServer(_redis.GetEndPoints().First());
        var keys = server.Keys(pattern: pattern).ToArray();
        if (keys.Length > 0)
        {
            await _database.KeyDeleteAsync(keys);
        }
    }

    public async Task<bool> ExistsAsync(string key)
    {
        return await _database.KeyExistsAsync(key);
    }
}

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
    Task RemoveAsync(string key);
    Task RemoveByPatternAsync(string pattern);
    Task<bool> ExistsAsync(string key);
}
