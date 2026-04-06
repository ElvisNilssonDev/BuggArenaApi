using BugArena.Application.DTOs.Leaderboard;

namespace BugArena.Application.Interfaces;

public interface ILeaderboardRepository
{
    Task<List<LeaderboardEntry>> GetGlobalTopAsync(int count = 50);
    Task<List<LeaderboardEntry>> GetWeeklyTopAsync(int count = 50);
    Task<int> GetGlobalRankAsync(Guid userId);
    Task<int> GetWeeklyRankAsync(Guid userId);
    Task<int> GetWeeklyPointsAsync(Guid userId);
    Task<int> GetChallengesSolvedAsync(Guid userId);
    Task<int> GetChallengesCreatedAsync(Guid userId);
    Task<int> GetAverageSolveTimeAsync(Guid userId);
    Task<string> GetFavoriteLanguageAsync(Guid userId);
}