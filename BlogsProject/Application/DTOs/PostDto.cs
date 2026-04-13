namespace BlogsProject.Application.DTOs;

public class CreatePostDto
{
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
    public List<string> Tags { get; set; } = [];
}

public class UpdatePostDto
{
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
    public List<string> Tags { get; set; } = [];
}

public class PostDto
{
    public string Id { get; set; } = null!;
    public string BlogId { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
    public List<string> Tags { get; set; } = [];
    public List<CommentDto> Comments { get; set; } = [];
    public DateTime CreatedAt { get; set; }
}

public class CommentDto
{
    public string UserId { get; set; } = null!;
    public string Body { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}

public class CreateCommentDto
{
    public string Body { get; set; } = null!;
    public string UserId { get; set; } = null!;
}