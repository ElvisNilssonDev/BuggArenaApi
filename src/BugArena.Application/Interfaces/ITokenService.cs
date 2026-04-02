using BugArena.Domain.Entities;

namespace BugArena.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}