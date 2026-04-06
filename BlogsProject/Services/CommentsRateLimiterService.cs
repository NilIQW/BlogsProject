using StackExchange.Redis;
using System;

namespace BlogsProject.Services;

public class CommentRateLimiterService
{
    private readonly RedisCacheService _redisCache;

    public CommentRateLimiterService(RedisCacheService redisCache)
    {
        _redisCache = redisCache;
    }

    public async Task<bool> CanCommentAsync(string userId, int maxComments = 5, int periodInSeconds = 60)
    {
        var db = _redisCache.GetDatabase();
        var key = $"comment:{userId}";

        // Increment counter atomically
        var count = await db.StringIncrementAsync(key);

        // Set expiration only on first comment
        if (count == 1)
            await db.KeyExpireAsync(key, TimeSpan.FromSeconds(periodInSeconds));

        return count <= maxComments;
    }
}