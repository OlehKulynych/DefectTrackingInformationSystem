using DTIS.Shared.Models;
using DTIS.WebApi.Data;
using DTIS.WebApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DTIS.WebApi.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly DataContext _context;

    public RoleRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Role>> GetAllRolesAsync()
    {
        return await _context.Roles.ToListAsync();
    }

    public async Task<bool> CreateRoleAsync(Role role)
    {
        if (role != null)
        {
            var oldRole = await GetRoleByNameAsync(role.Name);

            if (oldRole != null)
            {
                return false;
            }

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> UpdateAsync(Role role)
    {
        if (role == null)
        {
            return false;
        }

        _context.Update(role);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Role role)
    {
        if (role != null)
        {
            var userDb = await GetRoleByIdAsync(role.Id);

            _context.Remove(role!);
            await _context.SaveChangesAsync();

            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<Role?> GetRoleByIdAsync(int id)
    {
        return await _context.Roles.SingleOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Role?> GetRoleByNameAsync(string roleName)
    {
        return await _context.Roles.SingleOrDefaultAsync(r => r.Name == roleName);
    }
}
