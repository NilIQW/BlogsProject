using BlogsProject.Entities;
using BlogsProject.Repositories;

namespace BlogsProject.Services;

public class PostService
{
    private readonly IPostRepository _repo;
    private readonly RedisCacheService _cache;
    public PostService(IPostRepository repo, RedisCacheService cache)
    {
        _repo = repo;
        _cache = cache;
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

    public Task<Post> Create(Post post) =>
        _repo.Create(post);

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
        await _repo.AddComment(postId, comment);
        await _cache.InvalidatePostAsync(postId);
    }
}