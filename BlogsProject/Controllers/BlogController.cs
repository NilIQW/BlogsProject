using BlogsProject.DTOs;
using BlogsProject.Services;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> Create([FromBody] CreateBlogDto dto)
    {
        var created = await _service.Create(dto);
        return Ok(created);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var (blog, posts) = await _service.Get(id);
        if (blog == null) return NotFound();
        return Ok(new { blog, posts });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var blogs = await _service.GetAll();
        return Ok(blogs);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateBlogDto dto)
    {
        var updated = await _service.Update(id, dto);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _service.Delete(id);
        return NoContent();
    }
}