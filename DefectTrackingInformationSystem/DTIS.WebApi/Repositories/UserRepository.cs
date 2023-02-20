using DTIS.Shared.Models;
using DTIS.WebApi.Data;
using DTIS.WebApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DTIS.WebApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _context.Users
            .Include(x => x.Role)
            .ToListAsync();
    }

    public async Task<bool> CreateUserAsync(User user)
    {
        if (user != null)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }
        else
        {
            return false;
        }

    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        if (user == null)
        {
            return false;
        }

        _context.Update(user);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteUserAsync(User user)
    {
        if (user != null)
        {
            var userDb = await GetUserByEmailAsync(user.Email);

            _context.Remove(userDb!);
            await _context.SaveChangesAsync();

            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .Include(x => x.Role)
            .SingleOrDefaultAsync(u => u.Email == email);
    }
}
