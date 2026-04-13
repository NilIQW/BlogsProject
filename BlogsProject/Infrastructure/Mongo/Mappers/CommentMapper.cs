using BlogsProject.Domain.Entities;
using BlogsProject.Infrastructure.Mongo.Documents;

namespace BlogsProject.Infrastructure.Mongo.Mappers;

public static class CommentMapper
{
    public static Comment ToDomain(CommentDocument doc)
    {
        return new Comment
        {
            Id = doc.Id,
            PostId = doc.PostId,
            UserId = doc.UserId,
            Body = doc.Body,
            CreatedAt = doc.CreatedAt
        };
    }

    public static CommentDocument ToDocument(Comment entity)
    {
        return new CommentDocument
        {
            Id = entity.Id,
            PostId = entity.PostId,
            UserId = entity.UserId,
            Body = entity.Body,
            CreatedAt = entity.CreatedAt
        };
    }
}