namespace BugArena.Application.DTOs.Leaderboard;

public record UserStatsResponse(
    int GlobalRank,
    int WeeklyRank,
    int TotalPoints,
    int WeeklyPoints,
    int ChallengesSolved,
    int ChallengesCreated,
    int AverageSolveTimeSeconds,
    string FavoriteLanguage
);