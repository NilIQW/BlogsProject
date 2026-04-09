using BlogsProject.Entities;
using BlogsProject.Repositories;
using StackExchange.Redis;

namespace BlogsProject.Services;

public class PostService
{
    private readonly IPostRepository _repo;
    private readonly RedisCacheService _cache;
    private readonly CommentRateLimiterService _rateLimiter;
    public PostService(IPostRepository repo, RedisCacheService cache,CommentRateLimiterService rateLimiter)
    {
        _repo = repo;
        _cache = cache;
        _rateLimiter = rateLimiter;
    }

    public async Task<Post?> Get(string id)
    {
        var cached = await _cache.GetPostAsync(id);
        if (cached != null) return cached;

        var post = await _repo.GetById(id);
        if (post == null) return null;

        await _cache.SetPostAsync(post);

        return post;
    }
    public Task<List<Post>> GetByBlog(string blogId) =>
        _repo.GetByBlog(blogId);

    public async Task<Post> Create(Post post)
    {
        var created = await _repo.Create(post);

        var db = _cache.GetDatabase();

        var json = System.Text.Json.JsonSerializer.Serialize(created);

        await db.ExecuteAsync(
            "JSON.SET",
            $"post:{created.Id}",
            "$",
            json
        );

        return created;
    }
    
    public async Task<List<string>> Search(string text)
    {
        var db = _cache.GetDatabase();

        var result = await db.ExecuteAsync(
            "FT.SEARCH",
            "idx:blogposts",
            $"@title:{text} | @body:{text}"
        );

        var list = (RedisResult[])result;

        var posts = new List<string>();

        for (int i = 2; i < list.Length; i += 2)
        {
            posts.Add(list[i].ToString());
        }

        return posts;
    }
    public async Task Update(Post post)
    {
        await _repo.Update(post);
        await _cache.InvalidatePostAsync(post.Id);
    }
    public async Task Delete(string id){
       await _repo.Delete(id);
       await _cache.InvalidatePostAsync(id);
}
    public async Task AddComment(string postId, Comment comment)
    {
        // 1️⃣ Check rate limit
        var allowed = await _rateLimiter.CanCommentAsync(comment.UserId);
        if (!allowed)
            throw new Exception("Rate limit exceeded. Try again later.");

        await _repo.AddComment(postId, comment);

        await _cache.InvalidatePostAsync(postId);
    }
}