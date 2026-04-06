namespace BugArena.Application.DTOs.Profile;

public record UserSolutionListResponse(
    List<UserSolutionResponse> Items,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages
);