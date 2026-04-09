using BlogsProject.DTOs;
using BlogsProject.Entities;
using BlogsProject.Repositories;
using StackExchange.Redis;

namespace BlogsProject.Services;

public class PostService
{
    private readonly IPostRepository _repo;
    private readonly RedisCacheService _cache;
    private readonly CommentRateLimiterService _rateLimiter;

    public PostService(IPostRepository repo, RedisCacheService cache, CommentRateLimiterService rateLimiter)
    {
        _repo = repo;
        _cache = cache;
        _rateLimiter = rateLimiter;
    }

    public async Task<PostDto?> Get(string id)
    {
        var cached = await _cache.GetPostAsync(id);
        if (cached != null) return MapToDto(cached);

        var post = await _repo.GetById(id);
        if (post == null) return null;

        await _cache.SetPostAsync(post);
        return MapToDto(post);
    }

    public async Task<List<PostDto>> GetByBlog(string blogId)
    {
        var posts = await _repo.GetByBlog(blogId);
        return posts.Select(MapToDto).ToList();
    }

    public async Task<PostDto> Create(string blogId, CreatePostDto dto)
    {
        var post = new Post
        {
            BlogId = blogId,
            Title = dto.Title,
            Body = dto.Body,
            Tags = dto.Tags
        };

        var created = await _repo.Create(post);

        var db = _cache.GetDatabase();
        var json = System.Text.Json.JsonSerializer.Serialize(created);
        await db.ExecuteAsync("JSON.SET", $"post:{created.Id}", "$", json);

        return MapToDto(created);
    }

    public async Task<PostDto> Update(string id, UpdatePostDto dto)
    {
        var post = await _repo.GetById(id);
        if (post == null) throw new Exception("Post not found");

        post.Title = dto.Title;
        post.Body = dto.Body;
        post.Tags = dto.Tags;

        await _repo.Update(post);
        await _cache.InvalidatePostAsync(post.Id);

        return MapToDto(post);
    }

    public async Task Delete(string id)
    {
        await _repo.Delete(id);
        await _cache.InvalidatePostAsync(id);
    }

    public async Task<CommentDto> AddComment(string postId, Comment comment)
    {
        var allowed = await _rateLimiter.CanCommentAsync(comment.UserId);
        if (!allowed) throw new Exception("Rate limit exceeded");

        await _repo.AddComment(postId, comment);
        await _cache.InvalidatePostAsync(postId);

        return new CommentDto
        {
            UserId = comment.UserId,
            Body = comment.Body,
            CreatedAt = comment.CreatedAt
        };
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

    private static PostDto MapToDto(Post post) => new PostDto
    {
        Id = post.Id,
        BlogId = post.BlogId,
        Title = post.Title,
        Body = post.Body,
        Tags = post.Tags,
        CreatedAt = post.CreatedAt,
        Comments = post.Comments.Select(c => new CommentDto
        {
            UserId = c.UserId,
            Body = c.Body,
            CreatedAt = c.CreatedAt
        }).ToList()
    };
}