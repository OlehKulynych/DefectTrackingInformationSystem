using DTIS.Shared.Models;

namespace DTIS.WebApi.Repositories.Interfaces;

public interface IRoleRepository
{
    Task<List<Role>> GetAllRolesAsync();
    Task<bool> CreateRoleAsync(Role role);
    Task<bool> UpdateAsync(Role role);
    Task<bool> DeleteAsync(Role role);
    Task<Role?> GetRoleByIdAsync(int roleId);
    Task<Role?> GetRoleByNameAsync(string roleName);
}
