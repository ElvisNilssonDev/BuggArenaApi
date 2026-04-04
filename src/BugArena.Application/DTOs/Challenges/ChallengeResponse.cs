namespace BugArena.Application.DTOs.Challenges;

public record ChallengeResponse(
    Guid Id,
    Guid AuthorId,
    AuthorSummary Author,
    string Title,
    string Description,
    string BuggyCode,
    string ExpectedBehavior,
    string ActualBehavior,
    string? Hints,
    string Language,
    string Difficulty,
    string Status,
    int MaxPoints,
    int VoteCount,
    int SolutionCount,
    DateTime CreatedAt,
    DateTime? ClosedAt
);

public record AuthorSummary(
    Guid Id,
    string Username,
    string? AvatarUrl
);