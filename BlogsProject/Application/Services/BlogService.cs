using BlogsProject.Domain.Entities;
using BlogsProject.Domain.Interfaces;
using BlogsProject.Application.DTOs;

public class BlogService
{
    private readonly IBlogWriteRepository _write;
    private readonly IBlogReadRepository _read;

    public BlogService(
        IBlogWriteRepository write,
        IBlogReadRepository read)
    {
        _write = write;
        _read = read;
    }

    public async Task<BlogDto> Create(CreateBlogDto dto)
    {
        var blog = new Blog
        {
            Name = dto.Name,
            Description = dto.Description,
            UserId = dto.UserId
        };

        var created = await _write.Create(blog);

        return Map(created);
    }

    public async Task<BlogDto?> Get(string id)
    {
        var blog = await _read.GetById(id);
        return blog == null ? null : Map(blog);
    }

    public async Task<List<BlogDto>> GetAll()
    {
        var blogs = await _read.GetAll();
        return blogs.Select(Map).ToList();
    }

    public async Task<BlogDto?> Update(string id, UpdateBlogDto dto)
    {
        var blog = await _write.GetById(id);
        if (blog == null) return null;

        blog.Name = dto.Name;
        blog.Description = dto.Description;

        await _write.Update(blog);

        return Map(blog);
    }

    public async Task Delete(string id)
    {
        await _write.Delete(id);
    }

    private static BlogDto Map(Blog b) => new()
    {
        Id = b.Id,
        Name = b.Name,
        Description = b.Description,
        UserId = b.UserId,
        CreatedAt = b.CreatedAt
    };
}