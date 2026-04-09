namespace BlogsProject.DTOs;

public class CreateBlogDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string UserId { get; set; } = null!; 
}

public class UpdateBlogDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}

public class BlogDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}