using BlogApp.Data;
using BlogApp.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Repositories
{
    public class BlogPostCommentRepository : IBlogPostCommentRepository
    {
        private readonly BlogDbContext _blogDbContext;
        public BlogPostCommentRepository(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }

        public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
        {
            await _blogDbContext.Comments.AddAsync(blogPostComment);
            await _blogDbContext.SaveChangesAsync();
            return blogPostComment;
        }

        public async Task<BlogPostComment> DeleteAsync(string commentDesc, DateTime commentDate, string userId, Guid blogId)
        {
            // throw new NotImplementedException();
            var userIdGuid = Guid.Parse(userId);

            var comment = _blogDbContext.Comments.AsQueryable();

            comment = comment.Where(x =>
                x.BlogPostId == blogId &&
                x.Description == commentDesc &&
                x.UserId == userIdGuid);
            
            var commentAsked = await comment.ToListAsync();

            if (commentAsked != null && commentAsked.Any())
            {
                _blogDbContext.Comments.Remove(commentAsked[0]);
                await _blogDbContext.SaveChangesAsync();
                return commentAsked[0];
            }
            return null;
        }

        public async Task<IEnumerable<BlogPostComment>> DeleteCommentsByUserAsync(Guid userId)
        {
            var comments = await _blogDbContext.Comments.Where(x => x.UserId == userId).ToListAsync();

            foreach (var item in comments)
            {
                _blogDbContext.Comments.Remove(item);
            }
            await _blogDbContext.SaveChangesAsync();

            return comments;
        }

        public async Task<IEnumerable<BlogPostComment>> GetCommentsByBlogIdAsync(Guid blogPostId)
        {
            return await _blogDbContext.Comments.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }

    }
}