using BlogsProject.Domain.Entities;
using BlogsProject.Infrastructure.Mongo.Documents;

namespace BlogsProject.Infrastructure.Mongo.Mappers;

public static class PostMapper
{
    public static Post ToDomain(PostDocument doc)
    {
        return new Post
        {
            Id = doc.Id,
            BlogId = doc.BlogId,
            Title = doc.Title,
            Body = doc.Body,
            Tags = doc.Tags,
            CreatedAt = doc.CreatedAt,
            Comments = doc.Comments.Select(CommentMapper.ToDomain).ToList()
        };
    }

    public static PostDocument ToDocument(Post entity)
    {
        return new PostDocument
        {
            Id = entity.Id,
            BlogId = entity.BlogId,
            Title = entity.Title,
            Body = entity.Body,
            Tags = entity.Tags,
            CreatedAt = entity.CreatedAt,
            Comments = entity.Comments.Select(CommentMapper.ToDocument).ToList()
        };
    }
}