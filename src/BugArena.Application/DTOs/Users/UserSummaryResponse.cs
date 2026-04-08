namespace BugArena.Application.DTOs.Users;

public record UserSummaryResponse(
    Guid Id,
    string Username,
    string Email,
    string Role,
    int TotalPoints,
    string? AvatarUrl,
    DateTime CreatedAt
);