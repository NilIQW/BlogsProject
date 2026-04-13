using BlogsProject.Domain.Entities;
using BlogsProject.Infrastructure.Mongo.Documents;

namespace BlogsProject.Infrastructure.Mongo.Mappers;

public static class BlogMapper
{
    public static Blog ToDomain(BlogDocument doc)
    {
        return new Blog
        {
            Id = doc.Id,
            Name = doc.Name,
            Description = doc.Description,
            UserId = doc.UserId,
            CreatedAt = doc.CreatedAt
        };
    }

    public static BlogDocument ToDocument(Blog entity)
    {
        return new BlogDocument
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            UserId = entity.UserId,
            CreatedAt = entity.CreatedAt
        };
    }
}