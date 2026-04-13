using BlogsProject.Domain.Entities;

namespace BlogsProject.Domain.Interfaces;


public interface IBlogWriteRepository
{
    Task<Blog> Create(Blog blog);
    Task<Blog?> Update(Blog blog);
    Task Delete(string id);
    Task<Blog?> GetById(string id); // optional for validation
}