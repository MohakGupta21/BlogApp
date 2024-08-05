using BlogApp.Data;
using BlogApp.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BlogDbContext _blogDbContext;

        public TagRepository(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }

        public async Task<Tag?> GetAsync(Guid id)
        {
            return await _blogDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<Tag> AddAsync(Tag tag)
        {
            await _blogDbContext.Tags.AddAsync(tag);

            await _blogDbContext.SaveChangesAsync();

            return tag;
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingTag = await _blogDbContext.Tags.FindAsync(tag.Id);

            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                await _blogDbContext.SaveChangesAsync();

                return existingTag;
            }
            return null;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var tag = await _blogDbContext.Tags.FindAsync(id);

            if (tag != null)
            {
                _blogDbContext.Tags.Remove(tag);
                await _blogDbContext.SaveChangesAsync();

                // Show a success notification
                return tag;
            }
            return null;
        }

        public async Task<int> CountAsync()
        {
            // throw new NotImplementedException();
            return await _blogDbContext.Tags.CountAsync();
        }

        public async Task<IEnumerable<Tag>> GetAllAsync(string? searchQuery = null, string? sortBy = null, string? sortDirection = null, int pageNo = 1, int pageSize = 100)
        {
            // throw new NotImplementedException();
            var query = _blogDbContext.Tags.AsQueryable();

            // Filtering
            if (string.IsNullOrWhiteSpace(searchQuery) == false)
            {
                query = query.Where(x => x.Name.Contains(searchQuery) || x.DisplayName.Contains(searchQuery));
            }

            // Sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                var isDesc = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);

                if (string.Equals(sortBy, "Name", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.Name):query.OrderBy(x => x.Name);
                }

                if (string.Equals(sortBy, "DisplayName", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.DisplayName):query.OrderBy(x => x.DisplayName);
                }
            }
            // Pagination
            // Skip 0 take 5 -> Page 1 of 5 results
            // Skip 5 take next 5 -> Page 2 of 5 results
            var skipResults = (pageNo - 1)*pageSize;
            query = query.Skip(skipResults).Take(pageSize);


            return await query.ToListAsync();
        }
    }
}