using Microsoft.AspNetCore.Identity;

namespace BlogApp.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAll();
    }
}