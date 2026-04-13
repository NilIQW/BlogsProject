using StackExchange.Redis;

namespace BlogsProject.Infrastructure.Redis;

public class CommentRateLimiterService
{
    private readonly IDatabase _db;

    public CommentRateLimiterService(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    public async Task<bool> CanCommentAsync(string userId, int maxComments = 5, int periodInSeconds = 60)
    {
        var key = $"comment:{userId}";

        // Increment counter atomically
        var count = await _db.StringIncrementAsync(key);

        // Set expiration only on first comment
        if (count == 1)
            await _db.KeyExpireAsync(key, TimeSpan.FromSeconds(periodInSeconds));

        return count <= maxComments;
    }
}