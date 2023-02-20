using DTIS.Shared.Models;

namespace DTIS.WebApi.Repositories.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllUsersAsync();
    Task<bool> CreateUserAsync(User user);
    Task<bool> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(User user);
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByEmailAsync(string email);
    Task<bool> IsActivatedUserAsync(int id);
    Task<bool> ActivateUserAsync(int id);
}
