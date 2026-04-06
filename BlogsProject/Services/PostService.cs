using BlogsProject.Entities;
using BlogsProject.Repositories;

namespace BlogsProject.Services;

public class PostService
{
    private readonly IPostRepository _repo;

    public PostService(IPostRepository repo)
    {
        _repo = repo;
    }

    public Task<Post?> Get(string id) => _repo.GetById(id);

    public Task<List<Post>> GetByBlog(string blogId) =>
        _repo.GetByBlog(blogId);

    public Task<Post> Create(Post post) =>
        _repo.Create(post);

    public Task Update(Post post) =>
        _repo.Update(post);

    public Task Delete(string id) =>
        _repo.Delete(id);

    public Task AddComment(string postId, Comment comment) =>
        _repo.AddComment(postId, comment);
}