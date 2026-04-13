using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlogsProject.Infrastructure.Mongo.Documents;

public class CommentDocument
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonRepresentation(BsonType.ObjectId)]
    public string PostId { get; set; } = null!;

    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = null!;

    public string Body { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}