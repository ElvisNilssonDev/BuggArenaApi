using BugArena.Application.Interfaces;
using BugArena.Domain.Entities;
using BugArena.Domain.Enums;
using BugArena.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BugArena.Infrastructure.Repositories;

public class SolutionRepository : ISolutionRepository
{
    private readonly AppDbContext _context;

    public SolutionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Solution?> GetByIdAsync(Guid id)
    {
        return await _context.Solutions.FindAsync(id);
    }

    public async Task<Solution?> GetByIdWithDetailsAsync(Guid id)
    {
        return await _context.Solutions
            .Include(s => s.Solver)
            .Include(s => s.Challenge)
                .ThenInclude(c => c.Author)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Solution>> GetByChallengeIdAsync(Guid challengeId)
    {
        return await _context.Solutions
            .Include(s => s.Solver)
            .Include(s => s.Challenge)
            .Where(s => s.ChallengeId == challengeId)
            .OrderByDescending(s => s.SubmittedAt)
            .ToListAsync();
    }

    public async Task<int> GetAttemptCountAsync(Guid challengeId, Guid solverId)
    {
        return await _context.Solutions
            .CountAsync(s => s.ChallengeId == challengeId && s.SolverId == solverId);
    }

    public async Task<bool> HasApprovedSolutionsAsync(Guid challengeId)
    {
        return await _context.Solutions
            .AnyAsync(s => s.ChallengeId == challengeId && s.Status == SolutionStatus.Approved);
    }

    public async Task AddAsync(Solution solution)
    {
        await _context.Solutions.AddAsync(solution);
    }

    public async Task UpdateAsync(Solution solution)
    {
        _context.Solutions.Update(solution);
        await Task.CompletedTask;
    }
}