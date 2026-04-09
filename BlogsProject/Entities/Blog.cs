namespace BlogsProject.Entities;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Blog
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } 

    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}