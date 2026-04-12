using BugArena.Application.DTOs.Profile;
using BugArena.Domain.Entities;

namespace BugArena.Application.Interfaces;

public interface IProfileRepository
{
    Task<User?> GetByIdAsync(Guid userId);
    Task<int> GetChallengesCreatedCountAsync(Guid userId);
    Task<int> GetChallengesSolvedCountAsync(Guid userId);
    Task<List<UserChallengeResponse>> GetUserChallengesAsync(Guid userId, int page, int pageSize);
    Task<int> GetUserChallengesCountAsync(Guid userId);
    Task<List<UserSolutionResponse>> GetUserSolutionsAsync(Guid userId, int page, int pageSize);
    Task<int> GetUserSolutionsCountAsync(Guid userId);
}
