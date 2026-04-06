using BugArena.Application.DTOs.Leaderboard;
using BugArena.Application.Interfaces;
using BugArena.Domain.Enums;
using BugArena.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BugArena.Infrastructure.Repositories;

public class LeaderboardRepository : ILeaderboardRepository
{
    private readonly AppDbContext _context;

    public LeaderboardRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<LeaderboardEntry>> GetGlobalTopAsync(int count = 50)
    {
        var users = await _context.Users
            .OrderByDescending(u => u.TotalPoints)
            .Take(count)
            .Select(u => new
            {
                u.Id,
                u.Username,
                u.AvatarUrl,
                u.TotalPoints,
                ChallengesSolved = u.Solutions.Count(s => s.Status == SolutionStatus.Approved)
            })
            .ToListAsync();

        return users.Select((u, index) => new LeaderboardEntry(
            index + 1,
            u.Id,
            u.Username,
            u.AvatarUrl,
            u.TotalPoints,
            u.ChallengesSolved
        )).ToList();
    }

    public async Task<List<LeaderboardEntry>> GetWeeklyTopAsync(int count = 50)
    {
        var weekStart = GetWeekStart();

        var users = await _context.Solutions
            .Where(s => s.Status == SolutionStatus.Approved && s.SubmittedAt >= weekStart)
            .GroupBy(s => s.SolverId)
            .Select(g => new
            {
                UserId = g.Key,
                WeeklyPoints = g.Sum(s => s.PointsAwarded),
                ChallengesSolved = g.Count()
            })
            .OrderByDescending(x => x.WeeklyPoints)
            .Take(count)
            .ToListAsync();

        // Get user details
        var userIds = users.Select(u => u.UserId).ToList();
        var userDetails = await _context.Users
            .Where(u => userIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id);

        return users.Select((u, index) => new LeaderboardEntry(
            index + 1,
            u.UserId,
            userDetails[u.UserId].Username,
            userDetails[u.UserId].AvatarUrl,
            u.WeeklyPoints,
            u.ChallengesSolved
        )).ToList();
    }

    public async Task<int> GetGlobalRankAsync(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return 0;

        var rank = await _context.Users
            .CountAsync(u => u.TotalPoints > user.TotalPoints);

        return rank + 1;
    }

    public async Task<int> GetWeeklyRankAsync(Guid userId)
    {
        var weekStart = GetWeekStart();

        var userWeeklyPoints = await _context.Solutions
            .Where(s => s.SolverId == userId && s.Status == SolutionStatus.Approved && s.SubmittedAt >= weekStart)
            .SumAsync(s => s.PointsAwarded);

        if (userWeeklyPoints == 0) return 0;

        var higherCount = await _context.Solutions
            .Where(s => s.Status == SolutionStatus.Approved && s.SubmittedAt >= weekStart)
            .GroupBy(s => s.SolverId)
            .CountAsync(g => g.Sum(s => s.PointsAwarded) > userWeeklyPoints);

        return higherCount + 1;
    }

    public async Task<int> GetWeeklyPointsAsync(Guid userId)
    {
        var weekStart = GetWeekStart();

        return await _context.Solutions
            .Where(s => s.SolverId == userId && s.Status == SolutionStatus.Approved && s.SubmittedAt >= weekStart)
            .SumAsync(s => s.PointsAwarded);
    }

    public async Task<int> GetChallengesSolvedAsync(Guid userId)
    {
        return await _context.Solutions
            .CountAsync(s => s.SolverId == userId && s.Status == SolutionStatus.Approved);
    }

    public async Task<int> GetChallengesCreatedAsync(Guid userId)
    {
        return await _context.Challenges
            .CountAsync(c => c.AuthorId == userId && c.Status != ChallengeStatus.Closed);
    }

    public async Task<int> GetAverageSolveTimeAsync(Guid userId)
    {
        var hasAny = await _context.Solutions
            .AnyAsync(s => s.SolverId == userId && s.Status == SolutionStatus.Approved);

        if (!hasAny) return 0;

        return (int)await _context.Solutions
            .Where(s => s.SolverId == userId && s.Status == SolutionStatus.Approved)
            .AverageAsync(s => s.TimeToSolveSeconds);
    }

    public async Task<string> GetFavoriteLanguageAsync(Guid userId)
    {
        var favorite = await _context.Solutions
            .Where(s => s.SolverId == userId && s.Status == SolutionStatus.Approved)
            .Include(s => s.Challenge)
            .GroupBy(s => s.Challenge.Language)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .FirstOrDefaultAsync();

        return favorite.ToString() ?? "None";
    }

    // Week starts Monday 00:00 UTC
    private static DateTime GetWeekStart()
    {
        var now = DateTime.UtcNow;
        int diff = (7 + (now.DayOfWeek - DayOfWeek.Monday)) % 7;
        return now.AddDays(-diff).Date;
    }
}
