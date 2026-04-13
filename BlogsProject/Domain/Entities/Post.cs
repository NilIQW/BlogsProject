namespace BlogsProject.Domain.Entities;

public class Post : BaseEntity
{
    public string BlogId { get; set; } = null!;

    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;

    public List<string> Tags { get; set; } = new();

    // Domain allows it, but DB implementation may differ
    public List<Comment> Comments { get; set; } = new();
}