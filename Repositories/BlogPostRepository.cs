using BlogApp.Data;
using BlogApp.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Repositories
{
    public class BlogPostRepository:IBlogPostRepository
    {
        private readonly BlogDbContext _blogDbContext;
        public BlogPostRepository(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
            
        }
        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            // throw new Exception();
            return await _blogDbContext.BlogPosts.Include(e=>e.Tags).ToListAsync();
        }
        public async Task<BlogPost?> GetAsync(Guid id)
        {
            // throw new Exception();
            return await _blogDbContext.BlogPosts.Include(e=>e.Tags).FirstOrDefaultAsync(x=>x.Id==id);

        }

        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await _blogDbContext.AddAsync(blogPost);
            await _blogDbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            // throw new Exception();
            var existingBlogPost = await _blogDbContext.BlogPosts.Include(e=>e.Tags).FirstOrDefaultAsync(x=>x.Id == blogPost.Id);

            if(existingBlogPost!=null)
            {
                existingBlogPost.Id = blogPost.Id;
                existingBlogPost.Heading = blogPost.Heading;
                existingBlogPost.PageTitle = blogPost.PageTitle;
                existingBlogPost.Content = blogPost.Content;
                existingBlogPost.ShortDescription = blogPost.ShortDescription;
                existingBlogPost.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlogPost.UrlHandle = blogPost.UrlHandle;
                existingBlogPost.PublishedDate = blogPost.PublishedDate;
                existingBlogPost.Author = blogPost.Author;
                existingBlogPost.Visible = blogPost.Visible;
                existingBlogPost.Tags = blogPost.Tags;
                
                await _blogDbContext.SaveChangesAsync();
                return existingBlogPost;
            }
            return null;

        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            // throw new Exception();
            var blogToDelete = await _blogDbContext.BlogPosts.FindAsync(id);

            if(blogToDelete!=null)
            {
                _blogDbContext.BlogPosts.Remove(blogToDelete);
                await _blogDbContext.SaveChangesAsync();
                return blogToDelete;
            }
            return null;
            

        }
    
        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await _blogDbContext.BlogPosts.Include(e=>e.Tags).FirstOrDefaultAsync(x=>x.UrlHandle==urlHandle);
        }
    }
}