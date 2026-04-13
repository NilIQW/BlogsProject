using BlogsProject.Domain.Entities;

namespace BlogsProject.Infrastructure.Repositories;

public interface IBlogRepository
{
    Task<Blog> Create(Blog blog);
    Task<Blog?> GetById(string id);
    Task<List<Blog>> GetAll();
    Task<Blog?> Update(string id, Blog blog);
    Task Delete(string id);
}