namespace BlogsProject.Domain.Interfaces;

using BlogsProject.Domain.Entities;

public interface IPostReadRepository
{
    Task<Post?> GetById(string id);
    Task<List<Post>> GetByBlog(string blogId);
    Task<List<Post>> Search(string text);

}