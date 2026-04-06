namespace BugArena.Application.DTOs.Profile;

public record UserChallengeResponse(
    Guid Id,
    string Title,
    string Language,
    string Difficulty,
    int MaxPoints,
    int SolutionCount,
    int VoteCount,
    DateTime CreatedAt
);
