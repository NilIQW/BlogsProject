using BlogsProject.Application.Events;
using BlogsProject.Application.Messaging;
using BlogsProject.Domain.Entities;
using BlogsProject.Domain.Interfaces;
using BlogsProject.Infrastructure.Sql;
using Microsoft.EntityFrameworkCore;

namespace BlogsProject.Infrastructure.Sql.Repositories;

public class SqlPostWriteRepository : IPostWriteRepository
{
    private readonly AppDbContext _db;
    private readonly LocalMessageBus _bus; 

    public SqlPostWriteRepository(AppDbContext db, LocalMessageBus bus)
    {
        _db = db;
        _bus = bus;
    }

    public async Task<Post> Create(Post post)
    {
        _db.Posts.Add(post);
        await _db.SaveChangesAsync();
        
        await _bus.PublishAsync(new PostCreatedEvent(post));

        return post;
    }

    public async Task Update(Post post)
    {
        _db.Posts.Update(post);
        await _db.SaveChangesAsync();
    }

    public async Task Delete(string id)
    {
        var post = await _db.Posts.FindAsync(id);
        if (post == null) return;

        _db.Posts.Remove(post);
        await _db.SaveChangesAsync();
    }

    public async Task AddComment(string postId, Comment comment)
    {
        var post = await _db.Posts
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.Id == postId);

        if (post == null) return;

        post.Comments.Add(comment);

        await _db.SaveChangesAsync();
    }
}