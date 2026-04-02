using BugArena.Domain.Entities;

namespace BugArena.Application.Interfaces;

// Repository interface for managing user data, including retrieval by email, username, and ID, as well as adding new users.
public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByIdAsync(Guid id);
    Task AddAsync(User user);
}