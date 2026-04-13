namespace BlogsProject.Infrastructure.Redis;

using BlogsProject.Domain.Entities;
using System.Text.Json;
using StackExchange.Redis;

public class PostCache : IPostCache
{
    private readonly IDatabase _db;

    public PostCache(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    public async Task<Post?> Get(string id)
    {
        var value = await _db.StringGetAsync($"post:{id}");
        return value.HasValue
            ? JsonSerializer.Deserialize<Post>(value!)
            : null;
    }

    public async Task Set(Post post)
    {
        var json = JsonSerializer.Serialize(post);

        await _db.StringSetAsync(
            $"post:{post.Id}",
            json,
            TimeSpan.FromMinutes(10));
    }

    public async Task Remove(string id)
    {
        await _db.KeyDeleteAsync($"post:{id}");
    }
}