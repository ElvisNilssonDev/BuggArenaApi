namespace BugArena.Application.DTOs.Solutions;

public record SubmitSolutionRequest(
    string FixedCode,
    string Explanation,
    int TimeToSolveSeconds
);
