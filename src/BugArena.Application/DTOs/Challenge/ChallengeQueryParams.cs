namespace BugArena.Application.DTOs.Challenges;

public class ChallengeQueryParams
{
    public string? Language { get; set; }
    public string? Difficulty { get; set; }
    public string? Status { get; set; }
    public string SortBy { get; set; } = "newest";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
