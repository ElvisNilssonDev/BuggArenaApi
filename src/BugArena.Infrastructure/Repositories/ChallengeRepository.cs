using BugArena.Application.DTOs.Challenges;
using BugArena.Application.Interfaces;
using BugArena.Domain.Entities;
using BugArena.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BugArena.Infrastructure.Repositories;

public class ChallengeRepository : IChallengeRepository
{
    private readonly AppDbContext _context;

    public ChallengeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Challenge>> GetAllAsync(ChallengeQueryParams queryParams)
    {
        var query = _context.Challenges
            .Include(c => c.Author)
            .Include(c => c.Votes)
            .Include(c => c.Solutions)
            .AsQueryable();

        if (queryParams.Language.HasValue)
            query = query.Where(c => c.Language == queryParams.Language.Value);

        if (queryParams.Difficulty.HasValue)
            query = query.Where(c => c.Difficulty == queryParams.Difficulty.Value);

        if (queryParams.Status.HasValue)
            query = query.Where(c => c.Status == queryParams.Status.Value);

        var skip = (queryParams.Page - 1) * queryParams.PageSize;
        return await query.Skip(skip).Take(queryParams.PageSize).ToListAsync();
    }

    public async Task<int> CountAsync(ChallengeQueryParams queryParams)
    {
        var query = _context.Challenges.AsQueryable();

        if (queryParams.Language.HasValue)
            query = query.Where(c => c.Language == queryParams.Language.Value);

        if (queryParams.Difficulty.HasValue)
            query = query.Where(c => c.Difficulty == queryParams.Difficulty.Value);

        if (queryParams.Status.HasValue)
            query = query.Where(c => c.Status == queryParams.Status.Value);

        return await query.CountAsync();
    }

    public async Task<Challenge?> GetByIdAsync(Guid id)
        => await _context.Challenges.FindAsync(id);

    public async Task<Challenge?> GetByIdWithDetailsAsync(Guid id)
        => await _context.Challenges
            .Include(c => c.Author)
            .Include(c => c.Solutions)
            .Include(c => c.Votes)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task AddAsync(Challenge challenge)
        => await _context.Challenges.AddAsync(challenge);

    public async Task UpdateAsync(Challenge challenge)
        => _context.Challenges.Update(challenge);

    public async Task DeleteAsync(Challenge challenge)
        => _context.Challenges.Remove(challenge);

    public async Task<bool> ExistsAsync(Guid id)
        => await _context.Challenges.AnyAsync(c => c.Id == id);

    public async Task<int> GetSolutionCountAsync(Guid challengeId)
        => await _context.Solutions.CountAsync(s => s.ChallengeId == challengeId);
}