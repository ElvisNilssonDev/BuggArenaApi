using BugArena.Application.Interfaces;
using BugArena.Domain.Entities;
using BugArena.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BugArena.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
        => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<User?> GetByUsernameAsync(string username)
        => await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

    public async Task<User?> GetByIdAsync(Guid id)
        => await _context.Users.FindAsync(id);

    public async Task AddAsync(User user)
        => await _context.Users.AddAsync(user);

    public Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        return Task.CompletedTask;
    }

    public async Task<List<User>> GetAllAsync()
        => await _context.Users.ToListAsync();

    public async Task<bool> UpdateRoleAsync(Guid id, string role)
    {
        var user = await _context.Users.FindAsync(id);
        if (user is null) return false;

        user.Role = role;
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user is null) return false;

        _context.Users.Remove(user);
        return true;
    }
}