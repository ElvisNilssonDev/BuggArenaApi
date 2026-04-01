namespace BugArena.Application.DTOs.Auth;

public sealed record AuthResponse(
    string Token,
    DateTime ExpiresAtUtc,
    Guid UserId,
    string Username,
    string Email,
    string Role
);