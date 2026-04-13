using BlogsProject.Domain.Entities;
using BlogsProject.Domain.Interfaces;

namespace BlogsProject.Application.Events;

public class PostCreatedEvent : IDomainEvent
{
    public string PostId { get; }
    public string BlogId { get; }
    public string Title { get; }
    public string Body { get; }

    public DateTime OccurredAt { get; } = DateTime.UtcNow;

    public PostCreatedEvent(Post post)
    {
        PostId = post.Id;
        BlogId = post.BlogId;
        Title = post.Title;
        Body = post.Body;
    }
}