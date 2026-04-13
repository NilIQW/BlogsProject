namespace BlogsProject.Domain.Entities;

public class Comment : BaseEntity
{
    public string PostId { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string Body { get; set; } = null!;
}