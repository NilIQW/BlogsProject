namespace BlogsProject.Domain.Entities;

public class Blog : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string UserId { get; set; } = null!;
}