using BugArena.Domain.Enums;

namespace BugArena.Application.DTOs.Challenges;

public record CreateChallengeRequest(
    string Title,
    string Description,
    string BuggyCode,
    string ExpectedBehavior,
    string ActualBehavior,
    string? Hints,
    Language Language,
    Difficulty Difficulty
);