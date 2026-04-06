using BlogsProject.Entities;

namespace BlogsProject.Repositories;

using MongoDB.Driver;

public class MongoBlogRepository : IBlogRepository
{
    private readonly IMongoCollection<Blog> _blogs;

    public MongoBlogRepository(IMongoDatabase db)
    {
        _blogs = db.GetCollection<Blog>("blogs");
    }

    public async Task<Blog> Create(Blog blog)
    {
        await _blogs.InsertOneAsync(blog);
        return blog;
    }

    public async Task<Blog?> GetById(string id) =>
        await _blogs.Find(b => b.Id == id).FirstOrDefaultAsync();

    public async Task Delete(string id) =>
        await _blogs.DeleteOneAsync(b => b.Id == id);
}