namespace BugArena.Application.DTOs.Leaderboard;

public record LeaderboardEntry(
    int Rank,
    Guid UserId,
    string Username,
    string? AvatarUrl,
    int TotalPoints,
    int ChallengesSolved
);