using BugArena.Domain.Entities;

namespace BugArena.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByIdAsync(Guid id);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task<List<User>> GetAllAsync();
    Task<bool> UpdateRoleAsync(Guid id, string role);
    Task<bool> DeleteAsync(Guid id);
}