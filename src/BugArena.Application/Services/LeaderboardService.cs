using BugArena.Application.DTOs.Leaderboard;
using BugArena.Application.Interfaces;
using BugArena.Domain.Entities;

namespace BugArena.Application.Services;

public class LeaderboardService
{
    private readonly ILeaderboardRepository _leaderboardRepo;
    private readonly IUserRepository _userRepo;

    public LeaderboardService(ILeaderboardRepository leaderboardRepo, IUserRepository userRepo)
    {
        _leaderboardRepo = leaderboardRepo;
        _userRepo = userRepo;   
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
        var user = await _userRepo.GetByIdAsync(userId);
        if (user == null)
        {
            throw new Exception("User not found");
        };  

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
            user.TotalPoints,
            weeklyPoints,
            challengesSolved,
            challengesCreated,
            avgSolveTime,
            favoriteLanguage
        );
    }
}