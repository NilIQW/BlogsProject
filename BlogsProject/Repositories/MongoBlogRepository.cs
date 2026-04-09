using BlogsProject.Entities;
using BlogsProject.Repositories;
using MongoDB.Driver;

public class MongoBlogRepository : IBlogRepository
{
    private readonly IMongoCollection<Blog> _collection;

    public MongoBlogRepository(IMongoDatabase db)
    {
        _collection = db.GetCollection<Blog>("blogs");
    }

    public async Task<Blog> Create(Blog blog)
    {
        await _collection.InsertOneAsync(blog);
        return blog;
    }

    public async Task<Blog?> GetById(string id)
    {
        return await _collection.Find(b => b.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<Blog>> GetAll()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task<Blog?> Update(string id, Blog blog)
    {
        var result = await _collection.FindOneAndReplaceAsync(b => b.Id == id, blog);
        return result;
    }

    public async Task Delete(string id)
    {
        await _collection.DeleteOneAsync(b => b.Id == id);
    }
}