using BlogApp.Models.Domain;

namespace BlogApp.Repositories
{
    public interface IBlogPostLikeRepository
    {
        Task<int> GetTotalLikes(Guid blogPostId);

        Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId);

        Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike);

        Task<IEnumerable<BlogPostLike>> DeleteLikesOfUser(Guid userId);
    }
}