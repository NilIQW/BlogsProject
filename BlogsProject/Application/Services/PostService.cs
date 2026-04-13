using BlogsProject.Domain.Entities;
using BlogsProject.Domain.Interfaces;
using BlogsProject.Application.DTOs;
using BlogsProject.Application.Services;
using BlogsProject.Infrastructure.Redis;
using StackExchange.Redis;

public class PostService
{
    private readonly IPostWriteRepository _write;
    private readonly IPostReadRepository _read;
    private readonly IPostCache _cache;
    private readonly CommentRateLimiterService _rateLimiter;

    public PostService(
        IPostWriteRepository write,
        IPostReadRepository read,
        IPostCache cache,
        CommentRateLimiterService rateLimiter)
    {
        _write = write;
        _read = read;
        _cache = cache;
        _rateLimiter = rateLimiter;
    }

    public async Task<PostDto?> Get(string id)
    {
        var cached = await _cache.Get(id);
        if (cached != null)
            return Map(cached);

        var post = await _read.GetById(id);
        if (post == null) return null;

        await _cache.Set(post);

        return Map(post);
    }

    public async Task<List<PostDto>> GetByBlog(string blogId)
    {
        var posts = await _read.GetByBlog(blogId);
        return posts.Select(Map).ToList();
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

        var created = await _write.Create(post);

        await _cache.Remove(created.Id);

        return Map(created);
    }

    public async Task<PostDto> Update(string id, UpdatePostDto dto)
    {
        var post = await _read.GetById(id);
        if (post == null)
            throw new Exception("Post not found");

        post.Title = dto.Title;
        post.Body = dto.Body;
        post.Tags = dto.Tags;

        await _write.Update(post);

        await _cache.Remove(id);

        return Map(post);
    }

    public async Task Delete(string id)
    {
        await _write.Delete(id);
        await _cache.Remove(id);
    }

    public async Task<CommentDto> AddComment(string postId, CreateCommentDto dto)
    {
        var allowed = await _rateLimiter.CanCommentAsync(dto.UserId);
        if (!allowed)
            throw new Exception("Rate limit exceeded");

        var comment = new Comment
        {
            Id = Guid.NewGuid().ToString(),
            PostId = postId,
            UserId = dto.UserId,
            Body = dto.Body,
            CreatedAt = DateTime.UtcNow
        };

        await _write.AddComment(postId, comment);

        await _cache.Remove(postId);

        return new CommentDto
        {
            UserId = comment.UserId,
            Body = comment.Body,
            CreatedAt = comment.CreatedAt
        };
    }
    public async Task<List<PostDto>> Search(string text)
    {
        var posts = await _read.Search(text);
        return posts.Select(Map).ToList();
    }
    // ---------------- MAPPING ----------------
    private static PostDto Map(Post post) => new()
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