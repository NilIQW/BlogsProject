namespace BlogsProject.Infrastructure.Redis;

using BlogsProject.Domain.Entities;

public interface IPostCache
{
    Task<Post?> Get(string id);
    Task Set(Post post);
    Task Remove(string id);
}