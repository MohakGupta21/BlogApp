using BlogApp.Data;
using BlogApp.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Repositories
{
    public class BlogPostCommentRepository : IBlogPostCommentRepository
    {
        private readonly BlogDbContext _blogDbContext;
        private readonly AuthDbContext _dbContext;

        public BlogPostCommentRepository(BlogDbContext blogDbContext,AuthDbContext dbContext)
        {
            _blogDbContext = blogDbContext;
            _dbContext = dbContext;
        }

        public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
        {
            await _blogDbContext.Comments.AddAsync(blogPostComment);
            await _blogDbContext.SaveChangesAsync();
            return blogPostComment;
        }

        public async Task<BlogPostComment> DeleteAsync(string commentDesc, DateTime commentDate, string userId, Guid blogId, bool IsAdmin)
        {
            // throw new NotImplementedException();
            var userIdGuid = Guid.Parse(userId);

            var comment = _blogDbContext.Comments.AsQueryable();
            // var user = await _dbContext.Users.FirstOrDefaultAsync(x=>x.Id==userId);

            comment = comment.Where(x =>
                x.BlogPostId == blogId &&
                x.Description == commentDesc &&
                (x.UserId == userIdGuid || IsAdmin));
            
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