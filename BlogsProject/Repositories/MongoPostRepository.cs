using BlogsProject.Entities;
using MongoDB.Driver;

namespace BlogsProject.Repositories;

public class MongoPostRepository : IPostRepository
{
    private readonly IMongoCollection<Post> _posts;
    public MongoPostRepository(IMongoDatabase database)
    {
        _posts=database.GetCollection<Post>("posts");
    }
    public async Task<Post?> GetById(string id)=>
    await _posts.Find(p=>p.Id==id).FirstOrDefaultAsync();

    public async Task<List<Post>> GetByBlog(string blogId)=>
        await _posts.Find(p=>p.BlogId==blogId).ToListAsync();
    

    public async Task<Post> Create(Post post)
    {
        await _posts.InsertOneAsync(post);
        return post;
    }

    public Task Update(Post post)=>
    _posts.ReplaceOneAsync(p => p.Id == post.Id, post);

    public Task Delete(string id) =>
        _posts.DeleteOneAsync(p => p.Id == id);

    public async Task AddComment(string postId, Comment comment) =>
        await _posts.UpdateOneAsync(p=>p.Id == postId, Builders<Post>.Update.Push(p=>p.Comments, comment));
    
    public async Task DeleteByBlog(string blogId) =>
        await _posts.DeleteManyAsync(p => p.BlogId == blogId);
}