namespace BlogsProject.Domain.Interfaces;

using BlogsProject.Domain.Entities;

public interface IPostWriteRepository
{
    Task<Post> Create(Post post);
    Task Update(Post post);
    Task Delete(string id);
    Task AddComment(string postId, Comment comment);
}