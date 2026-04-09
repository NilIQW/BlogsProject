using BlogsProject.DTOs;
using BlogsProject.Entities;
using BlogsProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogsProject.Controllers;

[ApiController]
[Route("api/posts")]
public class PostsController : ControllerBase
{
    private readonly PostService _service;

    public PostsController(PostService service)
    {
        _service = service;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var post = await _service.Get(id);
        if (post == null) return NotFound();
        return Ok(post);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, UpdatePostDto dto)
    {
        try
        {
            var updated = await _service.Update(id, dto);
            return Ok(updated);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _service.Delete(id);
        return NoContent();
    }

    [HttpPost("blogs/{blogId}/posts")]
    public async Task<IActionResult> Create(string blogId, CreatePostDto dto)
    {
        var created = await _service.Create(blogId, dto);
        return Ok(created);
    }

    [HttpPost("{id}/comments")]
    public async Task<IActionResult> AddComment(string id, CommentDto dto)
    {
        var comment = new Comment
        {
            UserId = dto.UserId,
            Body = dto.Body,
            CreatedAt = dto.CreatedAt == default ? DateTime.UtcNow : dto.CreatedAt
        };

        var createdComment = await _service.AddComment(id, comment);
        return Ok(createdComment);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string q)
    {
        var results = await _service.Search(q);
        return Ok(results);
    }
}