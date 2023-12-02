using aspnetcore_web_api_cosmosdb.Models;

namespace aspnetcore_web_api_cosmosdb.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(User user);
        Task DeleteUserAsync(string userId);
        Task<User?> GetUserByIdAsync(string userId);
        Task<List<User>> GetUsersAsync();
        Task<User> UpdateUserAsync(User user);
    }
}
