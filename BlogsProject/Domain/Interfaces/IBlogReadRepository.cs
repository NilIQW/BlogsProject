namespace BlogsProject.Domain.Interfaces;

using BlogsProject.Domain.Entities;

public interface IBlogReadRepository
{
    Task<Blog?> GetById(string id);
    Task<List<Blog>> GetAll();
}