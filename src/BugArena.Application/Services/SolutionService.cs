using BugArena.Application.DTOs.Solutions;
using BugArena.Application.Interfaces;
using BugArena.Domain.Enums;
using BugArena.Domain.ValueObjects;

namespace BugArena.Application.Services;

public class SolutionService
{
    private readonly ISolutionRepository _solutionRepo;
    private readonly IChallengeRepository _challengeRepo;
    private readonly IUserRepository _userRepo;
    private readonly IUnitOfWork _unitOfWork;

    public SolutionService(
        ISolutionRepository solutionRepo,
        IChallengeRepository challengeRepo,
        IUserRepository userRepo,
        IUnitOfWork unitOfWork)
    {
        _solutionRepo = solutionRepo;
        _challengeRepo = challengeRepo;
        _userRepo = userRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<SolutionResponse> SubmitAsync(Guid challengeId, SubmitSolutionRequest request, Guid solverId)
    {
        var challenge = await _challengeRepo.GetByIdAsync(challengeId)
            ?? throw new KeyNotFoundException($"Challenge with ID '{challengeId}' was not found.");

        if (challenge.AuthorId == solverId)
            throw new UnauthorizedAccessException("You cannot submit a solution to your own challenge.");

        if (challenge.Status != ChallengeStatus.Published)
            throw new InvalidOperationException("This challenge is no longer accepting solutions.");

        int attemptCount = await _solutionRepo.GetAttemptCountAsync(challengeId, solverId);

        var solution = new Domain.Entities.Solution
        {
            SolverId = solverId,
            ChallengeId = challengeId,
            FixedCode = request.FixedCode,
            Explanation = request.Explanation,
            TimeToSolveSeconds = request.TimeToSolveSeconds,
            AttemptNumber = attemptCount + 1,
            Status = SolutionStatus.Pending
        };

        await _solutionRepo.AddAsync(solution);
        await _unitOfWork.SaveChangesAsync();

        var created = await _solutionRepo.GetByIdWithDetailsAsync(solution.Id);
        return MapToResponse(created!);
    }

    public async Task<List<SolutionResponse>> GetByChallengeIdAsync(Guid challengeId)
    {
        var solutions = await _solutionRepo.GetByChallengeIdAsync(challengeId);
        return solutions.Select(MapToResponse).ToList();
    }

    public async Task<SolutionResponse> ReviewAsync(Guid solutionId, ReviewSolutionRequest request, Guid reviewerId)
    {
        var solution = await _solutionRepo.GetByIdWithDetailsAsync(solutionId)
            ?? throw new KeyNotFoundException($"Solution with ID '{solutionId}' was not found.");

        if (solution.Challenge.AuthorId != reviewerId)
            throw new UnauthorizedAccessException("Only the challenge author can review solutions.");

        if (solution.Status != SolutionStatus.Pending)
            throw new InvalidOperationException("This solution has already been reviewed.");

        if (request.Status != SolutionStatus.Approved && request.Status != SolutionStatus.Rejected)
            throw new InvalidOperationException("Status must be Approved or Rejected.");

        solution.Status = request.Status;
        solution.Feedback = request.Feedback;

        if (request.Status == SolutionStatus.Approved)
        {
            bool hasApproved = await _solutionRepo.HasApprovedSolutionsAsync(solution.ChallengeId);

            int points = PointsCalculation.Calculate(
                solution.Challenge.Difficulty,
                isFirstSolver: !hasApproved,
                attemptNumber: solution.AttemptNumber,
                timeToSolveSeconds: solution.TimeToSolveSeconds
            );

            solution.PointsAwarded = points;

            var solver = await _userRepo.GetByIdAsync(solution.SolverId)
                ?? throw new KeyNotFoundException($"User with ID '{solution.SolverId}' was not found.");

            solver.TotalPoints += points;
            await _userRepo.UpdateAsync(solver);
        }

        await _solutionRepo.UpdateAsync(solution);
        await _unitOfWork.SaveChangesAsync();

        return MapToResponse(solution);
    }

    private static SolutionResponse MapToResponse(Domain.Entities.Solution s)
    {
        return new SolutionResponse(
            s.Id,
            s.SolverId,
            new SolverSummary(s.Solver.Id, s.Solver.Username, s.Solver.AvatarUrl),
            s.ChallengeId,
            s.Challenge.Title,
            s.FixedCode,
            s.Explanation,
            s.Status.ToString(),
            s.PointsAwarded,
            s.AttemptNumber,
            s.TimeToSolveSeconds,
            s.Feedback,
            s.SubmittedAt
        );
    }
}