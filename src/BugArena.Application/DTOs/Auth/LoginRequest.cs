namespace BugArena.Application.DTOs.Auth;

public sealed record LoginRequest(
    string Email,
    string Password
);