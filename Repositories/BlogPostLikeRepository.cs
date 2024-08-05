
using System.Collections.Generic;
using BlogApp.Data;
using BlogApp.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Repositories
{
    public class BlogPostLikeRepository : IBlogPostLikeRepository
    {
        private readonly BlogDbContext _blogDbContext;
        public BlogPostLikeRepository(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }

        public async Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike)
        {
            await _blogDbContext.BlogPostLike.AddAsync(blogPostLike);
            await _blogDbContext.SaveChangesAsync();
            return blogPostLike;
        }

        public async Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId)
        {
            return await _blogDbContext.BlogPostLike.Where(x=>x.BlogPostId == blogPostId).ToListAsync();
        }

        public async Task<int> GetTotalLikes(Guid blogPostId)
        {
            return await _blogDbContext.BlogPostLike.CountAsync(x => x.BlogPostId == blogPostId);
        }

        public async Task<IEnumerable<BlogPostLike>> DeleteLikesOfUser(Guid userId)
        {
            var models = await _blogDbContext.BlogPostLike.Where(x=>x.UserId == userId).ToListAsync();
            
            foreach (var item in models)
            {
                _blogDbContext.BlogPostLike.Remove(item);
            }
            await _blogDbContext.SaveChangesAsync();
            return models;
            
        }
    }
}