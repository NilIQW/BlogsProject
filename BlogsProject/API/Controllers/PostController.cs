using BlogsProject.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BlogsProject.API.Controllers;

[ApiController]
[Route("api/posts")]
public class PostController : ControllerBase
{
    private readonly PostService _service;

    public PostController(PostService service)
    {
        _service = service;
    }

    // GET POST
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _service.Get(id);
        return result == null ? NotFound() : Ok(result);
    }

    // GET POSTS BY BLOG
    [HttpGet("blog/{blogId}")]
    public async Task<IActionResult> GetByBlog(string blogId)
    {
        var result = await _service.GetByBlog(blogId);
        return Ok(result);
    }

    // CREATE POST
    [HttpPost("blog/{blogId}")]
    public async Task<IActionResult> Create(string blogId, CreatePostDto dto)
    {
        var result = await _service.Create(blogId, dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // UPDATE POST
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, UpdatePostDto dto)
    {
        var result = await _service.Update(id, dto);
        return Ok(result);
    }

    // DELETE POST
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _service.Delete(id);
        return NoContent();
    }

    // ADD COMMENT
    [HttpPost("{id}/comments")]
    public async Task<IActionResult> AddComment(string id, CreateCommentDto dto)
    {
        var result = await _service.AddComment(id, dto);
        return Ok(result);
    }

    // SEARCH
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string q)
    {
        var result = await _service.Search(q);
        return Ok(result);
    }
}