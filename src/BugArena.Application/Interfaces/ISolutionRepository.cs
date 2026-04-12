using BugArena.Domain.Entities;

namespace BugArena.Application.Interfaces;

public interface ISolutionRepository
{
    Task<Solution?> GetByIdAsync(Guid id);
    Task<Solution?> GetByIdWithDetailsAsync(Guid id);
    Task<List<Solution>> GetByChallengeIdAsync(Guid challengeId);
    Task<int> GetAttemptCountAsync(Guid challengeId, Guid solverId);
    Task<bool> HasApprovedSolutionsAsync(Guid challengeId);
    Task AddAsync(Solution solution);
    Task UpdateAsync(Solution solution);
}