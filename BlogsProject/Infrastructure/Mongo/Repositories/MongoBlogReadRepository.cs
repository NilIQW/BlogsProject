using BlogsProject.Domain.Interfaces;

namespace BlogsProject.Infrastructure.Mongo.Repositories;

using BlogsProject.Domain.Entities;
using BlogsProject.Infrastructure.Mongo.Documents;
using BlogsProject.Infrastructure.Mongo.Mappers;
using MongoDB.Driver;

public class MongoBlogReadRepository : IBlogReadRepository
{
    private readonly IMongoCollection<BlogDocument> _collection;

    public MongoBlogReadRepository(IMongoDatabase db)
    {
        _collection = db.GetCollection<BlogDocument>("blogs");
    }

    public async Task<Blog?> GetById(string id)
    {
        var doc = await _collection.Find(b => b.Id == id).FirstOrDefaultAsync();
        return doc == null ? null : BlogMapper.ToDomain(doc);
    }

    public async Task<List<Blog>> GetAll()
    {
        var docs = await _collection.Find(_ => true).ToListAsync();
        return docs.Select(BlogMapper.ToDomain).ToList();
    }
}