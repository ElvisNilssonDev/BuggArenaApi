using BugArena.Domain.Entities;

namespace BugArena.Application.Interfaces;

// Service for generating JWT tokens for authenticated users.
public interface ITokenService
{
    string GenerateToken(User user);
}