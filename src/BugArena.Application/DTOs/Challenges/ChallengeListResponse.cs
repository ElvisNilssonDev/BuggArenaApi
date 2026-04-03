namespace BugArena.Application.DTOs.Challenges;

public record ChallengeListResponse(
    List<ChallengeResponse> Items,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages
);