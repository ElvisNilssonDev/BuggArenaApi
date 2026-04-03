namespace BugArena.Application.DTOs.Solutions;

public record SolutionResponse(
    Guid Id,
    Guid SolverId,
    SolverSummary Solver,
    Guid ChallengeId,
    string ChallengeTitle,
    string FixedCode,
    string Explanation,
    string Status,
    int PointsAwarded,
    int AttemptNumber,
    int TimeToSolveSeconds,
    string? Feedback,
    DateTime SubmittedAt
);

public record SolverSummary(
    Guid Id,
    string Username,
    string? AvatarUrl
);