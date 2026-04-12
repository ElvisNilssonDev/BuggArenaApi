using BugArena.Domain.Enums;

namespace BugArena.Application.DTOs.Solutions;

public record ReviewSolutionRequest(
    SolutionStatus Status,
    string? Feedback
);