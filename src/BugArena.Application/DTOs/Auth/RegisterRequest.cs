namespace BugArena.Application.DTOs.Auth;

public sealed record RegisterRequest(
    string Username,
    string Email,
    string Password
);