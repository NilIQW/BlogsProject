using BlogsProject.Domain.Interfaces;

namespace BlogsProject.Infrastructure.Mongo.Repositories;

using BlogsProject.Domain.Entities;
using BlogsProject.Infrastructure.Mongo.Documents;
using BlogsProject.Infrastructure.Mongo.Mappers;
using MongoDB.Driver;

public class MongoPostReadRepository : IPostReadRepository
{
    private readonly IMongoCollection<PostDocument> _collection;

    public MongoPostReadRepository(IMongoDatabase db)
    {
        _collection = db.GetCollection<PostDocument>("posts");
    }

    public async Task<Post?> GetById(string id)
    {
        var doc = await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();
        return doc == null ? null : PostMapper.ToDomain(doc);
    }

    public async Task<List<Post>> GetByBlog(string blogId)
    {
        var docs = await _collection.Find(p => p.BlogId == blogId).ToListAsync();
        return docs.Select(PostMapper.ToDomain).ToList();
    }
    
    public async Task<List<Post>> Search(string text)
    {
        var db = _collection.Database.Client.GetDatabase("dummy"); 
        
        var filter = Builders<PostDocument>.Filter.Or(
            Builders<PostDocument>.Filter.Regex("Title", new MongoDB.Bson.BsonRegularExpression(text, "i")),
            Builders<PostDocument>.Filter.Regex("Body", new MongoDB.Bson.BsonRegularExpression(text, "i"))
        );

        var docs = await _collection.Find(filter).ToListAsync();

        return docs.Select(PostMapper.ToDomain).ToList();
    }
    
    public async Task Create(Post post)
    {
        var doc = PostMapper.ToDocument(post);
        await _collection.InsertOneAsync(doc);
    }

    public async Task Update(Post post)
    {
        var doc = PostMapper.ToDocument(post);

        await _collection.ReplaceOneAsync(
            p => p.Id == post.Id,
            doc);
    }

    public async Task Delete(string id)
    {
        await _collection.DeleteOneAsync(p => p.Id == id);
    }
}