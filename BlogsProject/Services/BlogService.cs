namespace BlogsProject.Services;

using BlogsProject.Entities;
using BlogsProject.Repositories;

public class BlogService
{
    private readonly IBlogRepository _blogs;
    private readonly IPostRepository _posts;

    public BlogService(IBlogRepository blogs, IPostRepository posts)
    {
        _blogs = blogs;
        _posts = posts;
    }

    public async Task<Blog> Create(Blog blog)
    {
        return await _blogs.Create(blog);
    }

    public async Task<(Blog?, List<Post>)> Get(string id)
    {
        var blog = await _blogs.GetById(id);
        if (blog == null) return (null, new List<Post>());

        var posts = await _posts.GetByBlog(id);
        return (blog, posts);
    }

    public async Task Delete(string id)
    {
        await _posts.DeleteByBlog(id);

        await _blogs.Delete(id);
    }
}