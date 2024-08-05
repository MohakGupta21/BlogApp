using BlogApp.Models.Domain;

namespace BlogApp.Repositories
{
    public interface ITagRepository
    {
        Task<Tag?> GetAsync(Guid id);

        Task<Tag> AddAsync(Tag tag);

        Task<Tag?> UpdateAsync(Tag tag);

        Task<Tag?> DeleteAsync(Guid id);

        Task<IEnumerable<Tag>> GetAllAsync(string? searchQuery=null,string? sortBy=null, string? sortDirection=null, int pageNo=1, int pageSize=100);

        Task<int> CountAsync();
    }
}