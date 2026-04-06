using BlogsProject.Entities;

namespace BlogsProject.Repositories;

public interface IBlogRepository
{
    Task<Blog> Create(Blog blog);
    Task<Blog?> GetById(string id);
    Task Delete(string id);
}