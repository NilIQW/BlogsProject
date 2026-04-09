namespace BlogsProject.DTOs;

public class CreatePostDto
{
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
    public List<string> Tags { get; set; } = [];
}