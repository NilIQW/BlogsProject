using BlogsProject.DTOs;
using BlogsProject.Entities;
using BlogsProject.Services;

namespace BlogsProject.Controllers;

using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> Update(string id, Post post)
    {
        post.Id = id;
        await _service.Update(post);
        return NoContent();
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
        var post = new Post
        {
            BlogId = blogId,
            Title = dto.Title,
            Body = dto.Body,
            Tags = dto.Tags
        };

        var created = await _service.Create(post);

        return Ok(created);
    }

    [HttpPost("{id}/comments")]
    public async Task<IActionResult> AddComment(string id, Comment comment)
    {
        await _service.AddComment(id, comment);
        return NoContent();
    }
    
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string q)
    {
        var results = await _service.Search(q);
        return Ok(results);
    }
}