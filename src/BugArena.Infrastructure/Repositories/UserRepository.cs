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

    public Task<User?> GetByEmailAsync(string email) =>
        _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public Task<User?> GetByUsernameAsync(string username) =>
        _context.Users.FirstOrDefaultAsync(u => u.Username == username);

    public async Task AddAsync(User user) =>
        await _context.Users.AddAsync(user);

    public Task<User?> GetByIdAsync(Guid id) =>
    _context.Users.FirstOrDefaultAsync(u => u.Id == id);
}