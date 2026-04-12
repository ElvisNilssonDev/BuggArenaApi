using System.Security.Claims;
using BugArena.Application.DTOs.Solutions;
using BugArena.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BugArena.API.Controllers;

[ApiController]
[Route("api")]
public class SolutionsController : ControllerBase
{
    private readonly SolutionService _solutionService;
    private readonly IValidator<SubmitSolutionRequest> _submitValidator;
    private readonly IValidator<ReviewSolutionRequest> _reviewValidator;

    public SolutionsController(
        SolutionService solutionService,
        IValidator<SubmitSolutionRequest> submitValidator,
        IValidator<ReviewSolutionRequest> reviewValidator)
    {
        _solutionService = solutionService;
        _submitValidator = submitValidator;
        _reviewValidator = reviewValidator;
    }

    // POST /api/challenges/{challengeId}/solutions
    [Authorize]
    [HttpPost("challenges/{challengeId}/solutions")]
    public async Task<IActionResult> Submit(Guid challengeId, [FromBody] SubmitSolutionRequest request)
    {
        var validation = await _submitValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors);

        try
        {
            var userId = GetUserId();
            var solution = await _solutionService.SubmitAsync(challengeId, request, userId);
            return Created($"/api/challenges/{challengeId}/solutions", solution);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // GET /api/challenges/{challengeId}/solutions
    [Authorize]
    [HttpGet("challenges/{challengeId}/solutions")]
    public async Task<IActionResult> GetByChallengeId(Guid challengeId)
    {
        var solutions = await _solutionService.GetByChallengeIdAsync(challengeId);
        return Ok(solutions);
    }

    // PUT /api/solutions/{id}/review
    [Authorize]
    [HttpPut("solutions/{id}/review")]
    public async Task<IActionResult> Review(Guid id, [FromBody] ReviewSolutionRequest request)
    {
        var validation = await _reviewValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors);

        try
        {
            var userId = GetUserId();
            var role = User.FindFirstValue(ClaimTypes.Role) ?? "User";
            var solution = await _solutionService.ReviewAsync(id, request, userId, role);
            return Ok(solution);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private Guid GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User ID not found in token.");
        return Guid.Parse(claim.Value);
    }
}