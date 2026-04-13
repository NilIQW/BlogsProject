using BlogsProject.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BlogsProject.API.Controllers;

[ApiController]
[Route("api/blogs")]
public class BlogsController : ControllerBase
{
    private readonly BlogService _service;

    public BlogsController(BlogService service)
    {
        _service = service;
    }

    // CREATE
    [HttpPost]
    public async Task<IActionResult> Create(CreateBlogDto dto)
    {
        var result = await _service.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // GET BY ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _service.Get(id);
        return result == null ? NotFound() : Ok(result);
    }

    // GET ALL
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAll();
        return Ok(result);
    }

    // UPDATE
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, UpdateBlogDto dto)
    {
        var result = await _service.Update(id, dto);
        return result == null ? NotFound() : Ok(result);
    }

    // DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _service.Delete(id);
        return NoContent();
    }
}