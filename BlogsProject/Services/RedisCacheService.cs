using BlogsProject.Entities;

namespace BlogsProject.Services;

using StackExchange.Redis;
using System.Text.Json;

public class RedisCacheService
{
    private readonly string _host;
    private readonly int _port;
    private readonly string? _password;

    private ConnectionMultiplexer? _connection;

    public RedisCacheService(string host, int port, string? password = null)
    {
        _host = host;
        _port = port;
        _password = password;
    }

    public void Connect()
    {
        _connection = ConnectionMultiplexer.Connect($"{_host}:{_port}, password={_password}");
    }

    public IDatabase GetDatabase()
    {
        if (_connection == null)
            throw new Exception("Redis not connected");

        return _connection.GetDatabase();
    }

    // ---------------- CACHE METHODS ----------------

    public async Task<Post?> GetPostAsync(string postId)
    {
        var db = GetDatabase();

        var value = await db.StringGetAsync($"post:{postId}");
        if (!value.HasValue) return null;

        return JsonSerializer.Deserialize<Post>(value!);
    }

    public async Task SetPostAsync(Post post)
    {
        var db = GetDatabase();

        var json = JsonSerializer.Serialize(post);

        await db.StringSetAsync(
            $"post:{post.Id}",
            json,
            TimeSpan.FromMinutes(5));
    }

    public async Task InvalidatePostAsync(string postId)
    {
        var db = GetDatabase();

        await db.KeyDeleteAsync($"post:{postId}");
    }
}