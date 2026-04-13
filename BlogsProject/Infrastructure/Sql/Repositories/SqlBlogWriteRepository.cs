using BlogsProject.Domain.Interfaces;

namespace BlogsProject.Infrastructure.Sql.Repositories;

using BlogsProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class SqlBlogWriteRepository : IBlogWriteRepository
{
    private readonly AppDbContext _db;

    public SqlBlogWriteRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Blog> Create(Blog blog)
    {
        _db.Blogs.Add(blog);
        await _db.SaveChangesAsync();
        return blog;
    }

    public async Task<Blog?> Update(Blog blog)
    {
        _db.Blogs.Update(blog);
        await _db.SaveChangesAsync();
        return blog;
    }

    public async Task Delete(string id)
    {
        var blog = await _db.Blogs.FindAsync(id);
        if (blog == null) return;

        _db.Blogs.Remove(blog);
        await _db.SaveChangesAsync();
    }

    public async Task<Blog?> GetById(string id)
    {
        return await _db.Blogs.FindAsync(id);
    }
}