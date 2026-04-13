using BlogsProject.Application.Events;

namespace BlogsProject.Application.Handlers;

using BlogsProject.Domain.Entities;
using BlogsProject.Domain.Interfaces;

public class PostCreatedEventHandler
{
    private readonly IPostReadRepository _readRepo;

    public PostCreatedEventHandler(IPostReadRepository readRepo)
    {
        _readRepo = readRepo;
    }

    public async Task Handle(PostCreatedEvent domainEvent)
    {
        var post = new Post
        {
            Id = domainEvent.PostId,
            BlogId = domainEvent.BlogId,
            Title = domainEvent.Title,
            Body = domainEvent.Body,
            CreatedAt = DateTime.UtcNow
        };

        await _readRepo.Create(post);
    }
}