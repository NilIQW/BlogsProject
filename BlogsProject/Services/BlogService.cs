using BlogsProject.DTOs;
using BlogsProject.Entities;
using BlogsProject.Repositories;

public class BlogService
{
    private readonly IBlogRepository _blogs;
    private readonly IPostRepository _posts; // Optional if posts are linked

    public BlogService(IBlogRepository blogs, IPostRepository posts)
    {
        _blogs = blogs;
        _posts = posts;
    }

    // CREATE
    public async Task<BlogDto> Create(CreateBlogDto dto)
    {
        var blog = new Blog
        {
            Name = dto.Name,
            Description = dto.Description,
            UserId = dto.UserId
        };

        var created = await _blogs.Create(blog);

        return MapToDto(created);
    }

    // GET ONE
    public async Task<(BlogDto?, List<Post>)> Get(string id)
    {
        var blog = await _blogs.GetById(id);
        if (blog == null) return (null, new List<Post>());

        var posts = await _posts.GetByBlog(id); // optional
        return (MapToDto(blog), posts);
    }

    // GET ALL
    public async Task<List<BlogDto>> GetAll()
    {
        var blogs = await _blogs.GetAll();
        return blogs.Select(MapToDto).ToList();
    }

    // UPDATE
    public async Task<BlogDto?> Update(string id, UpdateBlogDto dto)
    {
        var existing = await _blogs.GetById(id);
        if (existing == null) return null;

        existing.Name = dto.Name;
        existing.Description = dto.Description;

        var updated = await _blogs.Update(id, existing);
        return updated != null ? MapToDto(updated) : null;
    }

    // DELETE
    public async Task Delete(string id)
    {
        await _posts.DeleteByBlog(id); // optional if you want cascade delete
        await _blogs.Delete(id);
    }

    private BlogDto MapToDto(Blog blog) => new BlogDto
    {
        Id = blog.Id!,
        Name = blog.Name,
        Description = blog.Description,
        UserId = blog.UserId,
        CreatedAt = blog.CreatedAt
    };
}