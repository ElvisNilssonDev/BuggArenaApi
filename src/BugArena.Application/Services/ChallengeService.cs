using BugArena.Application.DTOs.Challenges;
using BugArena.Application.Interfaces;
using BugArena.Domain.Entities;
using BugArena.Domain.Enums;

namespace BugArena.Application.Services;

public class ChallengeService
{
    private readonly IChallengeRepository _challengeRepo;
    private readonly IUnitOfWork _unitOfWork;

    public ChallengeService(IChallengeRepository challengeRepo, IUnitOfWork unitOfWork)
    {
        _challengeRepo = challengeRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<ChallengeListResponse> GetAllAsync(ChallengeQueryParams queryParams)
    {
        var challenges = await _challengeRepo.GetAllAsync(queryParams);
        var totalCount = await _challengeRepo.CountAsync(queryParams);
        var totalPages = (int)Math.Ceiling(totalCount / (double)queryParams.PageSize);

        var items = challenges.Select(c => new ChallengeResponse(
            c.Id,
            c.AuthorId,
            new AuthorSummary(c.Author.Id, c.Author.Username, c.Author.AvatarUrl),
            c.Title,
            c.Description,
            c.BuggyCode,
            c.ExpectedBehavior,
            c.ActualBehavior,
            c.Hints,
            c.Language.ToString(),
            c.Difficulty.ToString(),
            c.Status.ToString(),
            c.MaxPoints,
            c.Votes.Count,
            c.Solutions.Count,
            c.CreatedAt,
            c.ClosedAt
        )).ToList();

        return new ChallengeListResponse(items, totalCount, queryParams.Page, queryParams.PageSize, totalPages);
    }

    public async Task<ChallengeResponse?> GetByIdAsync(Guid id)
    {
        var challenge = await _challengeRepo.GetByIdWithDetailsAsync(id);
        if (challenge is null) return null;

        return new ChallengeResponse(
            challenge.Id,
            challenge.AuthorId,
            new AuthorSummary(challenge.Author.Id, challenge.Author.Username, challenge.Author.AvatarUrl),
            challenge.Title,
            challenge.Description,
            challenge.BuggyCode,
            challenge.ExpectedBehavior,
            challenge.ActualBehavior,
            challenge.Hints,
            challenge.Language.ToString(),
            challenge.Difficulty.ToString(),
            challenge.Status.ToString(),
            challenge.MaxPoints,
            challenge.Votes.Count,
            challenge.Solutions.Count,
            challenge.CreatedAt,
            challenge.ClosedAt
        );
    }

    public async Task<ChallengeResponse> CreateAsync(CreateChallengeRequest dto, Guid authorId)
    {
        var challenge = new Challenge
        {
            Id = Guid.NewGuid(),
            AuthorId = authorId,
            Title = dto.Title,
            Description = dto.Description,
            BuggyCode = dto.BuggyCode,
            ExpectedBehavior = dto.ExpectedBehavior,
            ActualBehavior = dto.ActualBehavior,
            Hints = dto.Hints,
            Language = dto.Language,
            Difficulty = dto.Difficulty,
            MaxPoints = dto.Difficulty switch
            {
                Difficulty.Easy => 100,
                Difficulty.Medium => 250,
                Difficulty.Hard => 500,
                _ => 100
            },
            Status = ChallengeStatus.Published,
            CreatedAt = DateTime.UtcNow
        };

        await _challengeRepo.AddAsync(challenge);
        await _unitOfWork.SaveChangesAsync();

        return (await GetByIdAsync(challenge.Id))!;
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateChallengeRequest dto, Guid requestingUserId)
    {
        var challenge = await _challengeRepo.GetByIdAsync(id);
        if (challenge is null) return false;
        if (challenge.AuthorId != requestingUserId)
            throw new UnauthorizedAccessException("You can only edit your own challenges.");

        challenge.Title = dto.Title;
        challenge.Description = dto.Description;
        challenge.ExpectedBehavior = dto.ExpectedBehavior;
        challenge.ActualBehavior = dto.ActualBehavior;
        challenge.Hints = dto.Hints;
        challenge.Difficulty = dto.Difficulty;

        await _challengeRepo.UpdateAsync(challenge);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid requestingUserId, string role)
    {
        var challenge = await _challengeRepo.GetByIdAsync(id);
        if (challenge is null) return false;

        if (role != "Admin" && challenge.AuthorId != requestingUserId)
            throw new UnauthorizedAccessException("You can only delete your own challenges.");

        await _challengeRepo.DeleteAsync(challenge);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CloseAsync(Guid id)
    {
        var challenge = await _challengeRepo.GetByIdAsync(id);
        if (challenge is null) return false;

        challenge.Status = ChallengeStatus.Closed;
        challenge.ClosedAt = DateTime.UtcNow;
        await _challengeRepo.UpdateAsync(challenge);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}