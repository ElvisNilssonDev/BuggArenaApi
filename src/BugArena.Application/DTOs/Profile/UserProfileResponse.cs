namespace BugArena.Application.DTOs.Profile;

public record UserProfileResponse(
    Guid Id,
    string Username,
    string? AvatarUrl,
    int TotalPoints,
    string Role,
    DateTime CreatedAt,
    int ChallengesCreated,
    int ChallengesSolved
);