using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlogsProject.Infrastructure.Mongo.Documents;

public class PostDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    [BsonRepresentation(BsonType.ObjectId)]
    public string BlogId { get; set; } = null!;

    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;

    public List<string> Tags { get; set; } = new();

    public List<CommentDocument> Comments { get; set; } = new();

    public DateTime CreatedAt { get; set; }
}