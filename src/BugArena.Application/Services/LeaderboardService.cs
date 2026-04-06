using BugArena.Application.DTOs.Leaderboard;
using BugArena.Application.Interfaces;

namespace BugArena.Application.Services;

public class LeaderboardService
{
    private readonly ILeaderboardRepository _leaderboardRepo;

    public LeaderboardService(ILeaderboardRepository leaderboardRepo)
    {
        _leaderboardRepo = leaderboardRepo;
    }

    public async Task<List<LeaderboardEntry>> GetGlobalAsync()
    {
        return await _leaderboardRepo.GetGlobalTopAsync(50);
    }

    public async Task<List<LeaderboardEntry>> GetWeeklyAsync()
    {
        return await _leaderboardRepo.GetWeeklyTopAsync(50);
    }

    public async Task<UserStatsResponse> GetUserStatsAsync(Guid userId)
    {
        var globalRank = await _leaderboardRepo.GetGlobalRankAsync(userId);
        var weeklyRank = await _leaderboardRepo.GetWeeklyRankAsync(userId);
        var weeklyPoints = await _leaderboardRepo.GetWeeklyPointsAsync(userId);
        var challengesSolved = await _leaderboardRepo.GetChallengesSolvedAsync(userId);
        var challengesCreated = await _leaderboardRepo.GetChallengesCreatedAsync(userId);
        var avgSolveTime = await _leaderboardRepo.GetAverageSolveTimeAsync(userId);
        var favoriteLanguage = await _leaderboardRepo.GetFavoriteLanguageAsync(userId);

        return new UserStatsResponse(
            globalRank,
            weeklyRank,
            0, // will be filled from user entity
            weeklyPoints,
            challengesSolved,
            challengesCreated,
            avgSolveTime,
            favoriteLanguage
        );
    }
}