using BugArena.Application.DTOs.Challenges;
using BugArena.Domain.Entities;

namespace BugArena.Application.Interfaces;

public interface IChallengeRepository
{
    Task<IEnumerable<Challenge>> GetAllAsync(ChallengeQueryParams queryParams);
    Task<Challenge?> GetByIdAsync(Guid id);
    Task<Challenge?> GetByIdWithDetailsAsync(Guid id);
    Task AddAsync(Challenge challenge);
    Task UpdateAsync(Challenge challenge);
    Task DeleteAsync(Challenge challenge);
    Task<bool> ExistsAsync(Guid id);
    Task<int> GetSolutionCountAsync(Guid challengeId);
    Task<int> CountAsync(ChallengeQueryParams queryParams);
}