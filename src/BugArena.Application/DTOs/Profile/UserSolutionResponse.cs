namespace BugArena.Application.DTOs.Profile;

public record UserSolutionResponse(
    Guid Id,
    Guid ChallengeId,
    string ChallengeTitle,
    string Status,
    int PointsAwarded,
    int AttemptNumber,
    DateTime SubmittedAt
);