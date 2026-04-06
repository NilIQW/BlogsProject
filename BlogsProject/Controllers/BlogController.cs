using BlogsProject.Entities;
using BlogsProject.Repositories;
using BlogsProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogsProject.Controllers;

[ApiController]
[Route("api/blogs")]
public class BlogsController : ControllerBase
{
    private readonly BlogService _service;

    public BlogsController(BlogService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Blog blog)
    {
        var created = await _service.Create(blog);
        return Ok(created);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var (blog, posts) = await _service.Get(id);
        if (blog == null) return NotFound();
        return Ok(new { blog, posts });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _service.Delete(id);
        return NoContent();
    }
    
}