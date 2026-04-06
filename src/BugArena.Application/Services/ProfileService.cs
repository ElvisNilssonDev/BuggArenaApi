using BugArena.Application.DTOs.Challenges;
using BugArena.Application.Interfaces;
using static System.Net.Mime.MediaTypeNames;

ususing BugArena.Application.DTOs.Challenges;
using BugArena.Application.DTOs.Profile;
using BugArena.Application.Interfaces;

namespace BugArena.Application.Services;

public class ProfileService
{
    private readonly IProfileRepository _profileRepo;
    private readonly ILeaderboardRepository _leaderboardRepo;

    public ProfileService(IProfileRepository profileRepo, ILeaderboardRepository leaderboardRepo)
    {
        _profileRepo = profileRepo;
        _leaderboardRepo = leaderboardRepo;
    }

    public async Task<UserProfileResponse> GetProfileAsync(Guid userId)
    {
        var user = await _profileRepo.GetByIdAsync(userId)
            ?? throw new KeyNotFoundException($"User with ID '{userId}' was not found.");

        var challengesCreated = await _profileRepo.GetChallengesCreatedCountAsync(userId);
        var challengesSolved = await _profileRepo.GetChallengesSolvedCountAsync(userId);

        return new UserProfileResponse(
            user.Id,
            user.Username,
            user.AvatarUrl,
            user.TotalPoints,
            user.Role,
            user.CreatedAt,
            challengesCreated,
            challengesSolved
        );
    }

    public async Task<ChallengeListResponse> GetUserChallengesAsync(Guid userId, int page = 1, int pageSize = 10)
    {
        var items = await _profileRepo.GetUserChallengesAsync(userId, page, pageSize);
        var totalCount = await _profileRepo.GetUserChallengesCountAsync(userId);
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        // Map to ChallengeResponse format for consistency
        var challengeResponses = items.Select(c => new ChallengeResponse(
            c.Id,
            userId,
            new AuthorSummary(userId, "", null), // minimal — frontend already knows the user
            c.Title,
            "",  // description not needed in list
            "",  // buggyCode not needed in list
            "",  // expectedBehavior not needed in list
            "",  // actualBehavior not needed in list
            null,
            c.Language,
            c.Difficulty,
            "Published",
            c.MaxPoints,
            c.VoteCount,
            c.SolutionCount,
            c.CreatedAt,
            null
        )).ToList();

        return new ChallengeListResponse(challengeResponses, totalCount, page, pageSize, totalPages);
    }

    public async Task<UserSolutionListResponse> GetUserSolutionsAsync(Guid userId, int page = 1, int pageSize = 10)
    {
        var items = await _profileRepo.GetUserSolutionsAsync(userId, page, pageSize);
        var totalCount = await _profileRepo.GetUserSolutionsCountAsync(userId);
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new UserSolutionListResponse(items, totalCount, page, pageSize, totalPages);
    }
}