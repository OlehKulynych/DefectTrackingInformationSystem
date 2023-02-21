﻿using DTIS.Shared.Models;
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

    public async Task<List<User>> GetAllUsersAsync()
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

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .Include(x => x.Role)
            .SingleOrDefaultAsync(r => r.Id == id);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .Include(x => x.Role)
            .SingleOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> IsActivatedUserAsync(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
        {
            throw new Exception();
        }

        return user.IsActivated;
    }

    public async Task<bool> ActivateUserAsync(int id)
    {
        var user = await GetUserByIdAsync(id);

        if (user == null)
        {
            return false;
        }

        user.IsActivated = true;

        var closed = await _context.SaveChangesAsync();

        return closed > 0;
    }

    public async Task<bool> DeActivateUserAsync(int id)
    {
        var user = await GetUserByIdAsync(id);

        if (user == null)
        {
            return false;
        }

        user.IsActivated = false;

        var closed = await _context.SaveChangesAsync();

        return closed > 0;
    }

    public async Task<bool> SetUserRole(User user, Role role)
    {
        var userInDB = await GetUserByIdAsync(user.Id);
        var roleInDB = await _context.Roles.FirstOrDefaultAsync(x => x.Id == role.Id);

        if (user == null || role == null || userInDB == null || roleInDB == null)
        {
            return false;
        }

        userInDB.Role = roleInDB;
        var result = await _context.SaveChangesAsync();

        return result > 0;
    }

    public async Task<bool> IsUserExistAsync(int id)
    {
        var users = _context.Users.Where(x => x.Id == id).AsNoTracking();
        var user = await users.FirstOrDefaultAsync(x => x.Id == id);

        return user != null;
    }
}
