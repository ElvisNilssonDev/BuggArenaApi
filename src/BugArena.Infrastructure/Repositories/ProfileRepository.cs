using BugArena.Application.DTOs.Profile;
using BugArena.Application.Interfaces;
using BugArena.Domain.Entities;
using BugArena.Domain.Enums;
using BugArena.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BugArena.Infrastructure.Repositories;

public class ProfileRepository : IProfileRepository
{
    private readonly AppDbContext _context;

    public ProfileRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid userId)
    {
        return await _context.Users.FindAsync(userId);
    }

    public async Task<int> GetChallengesCreatedCountAsync(Guid userId)
    {
        return await _context.Challenges
            .CountAsync(c => c.AuthorId == userId && c.Status != ChallengeStatus.Closed);
    }

    public async Task<int> GetChallengesSolvedCountAsync(Guid userId)
    {
        return await _context.Solutions
            .CountAsync(s => s.SolverId == userId && s.Status == SolutionStatus.Approved);
    }

    public async Task<List<UserChallengeResponse>> GetUserChallengesAsync(Guid userId, int page, int pageSize)
    {
        return await _context.Challenges
            .Where(c => c.AuthorId == userId && c.Status != ChallengeStatus.Closed)
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new UserChallengeResponse(
                c.Id,
                c.Title,
                c.Language.ToString(),
                c.Difficulty.ToString(),
                c.MaxPoints,
                c.Solutions.Count,
                c.Votes.Count(v => v.IsUpvote) - c.Votes.Count(v => !v.IsUpvote),
                c.CreatedAt
            ))
            .ToListAsync();
    }

    public async Task<int> GetUserChallengesCountAsync(Guid userId)
    {
        return await _context.Challenges
            .CountAsync(c => c.AuthorId == userId && c.Status != ChallengeStatus.Closed);
    }

    public async Task<List<UserSolutionResponse>> GetUserSolutionsAsync(Guid userId, int page, int pageSize)
    {
        return await _context.Solutions
            .Where(s => s.SolverId == userId)
            .OrderByDescending(s => s.SubmittedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new UserSolutionResponse(
                s.Id,
                s.ChallengeId,
                s.Challenge.Title,
                s.Status.ToString(),
                s.PointsAwarded,
                s.AttemptNumber,
                s.SubmittedAt
            ))
            .ToListAsync();
    }

    public async Task<int> GetUserSolutionsCountAsync(Guid userId)
    {
        return await _context.Solutions
            .CountAsync(s => s.SolverId == userId);
    }
}