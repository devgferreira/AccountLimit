using Authenticate.API.Models;

namespace Authenticate.API.Repository
{
    public interface IInMemoryUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task CreateAsync(User user);
    }
}
