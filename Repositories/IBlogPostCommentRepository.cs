using BlogApp.Models.Domain;

namespace BlogApp.Repositories
{
    public interface IBlogPostCommentRepository
    {
        Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment);

        Task<IEnumerable<BlogPostComment>> GetCommentsByBlogIdAsync(Guid blogPostId);

        Task<IEnumerable<BlogPostComment>> DeleteCommentsByUserAsync(Guid userId);

        Task<BlogPostComment> DeleteAsync(string commentDesc, DateTime commentDate,string userId,Guid blogId);

    }
}