using BugArena.Application.DTOs.Auth;
using BugArena.Domain.Entities;

namespace BugArena.Application.Interfaces;

public interface ITokenService
{
    AuthResponse CreateToken(User user);
}