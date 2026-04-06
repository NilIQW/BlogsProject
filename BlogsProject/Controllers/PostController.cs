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

    [HttpPost("{id}/comments")]
    public async Task<IActionResult> AddComment(string id, Comment comment)
    {
        await _service.AddComment(id, comment);
        return NoContent();
    }
}