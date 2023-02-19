using DTIS.Shared.Models;

namespace DTIS.WebApi.Repositories.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllUsers();
    Task<bool> CreateUserAsync(User user);
    Task<bool> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(User user);
    Task<User?> GetUserByEmailAsync(string email);

}
