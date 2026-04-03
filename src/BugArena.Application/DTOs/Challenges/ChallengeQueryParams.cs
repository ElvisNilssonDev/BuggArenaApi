using BugArena.Domain.Enums;

namespace BugArena.Application.DTOs.Challenges;

public class ChallengeQueryParams
{
    public Language? Language { get; set; }
    public Difficulty? Difficulty { get; set; }
    public ChallengeStatus? Status { get; set; }
    public string SortBy { get; set; } = "newest";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}