using BugArena.Domain.Enums;

namespace BugArena.Application.DTOs.Challenges;

public record UpdateChallengeRequest(
    string Title,
    string Description,
    string ExpectedBehavior,
    string ActualBehavior,
    string? Hints,
    Difficulty Difficulty
);