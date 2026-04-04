using System.Security.Claims;
using BugArena.Application.DTOs.Solutions;
using BugArena.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BugArena.API.Controllers;

[ApiController]
[Route("api")]
public class SolutionsController : ControllerBase
{
    private readonly SolutionService _solutionService;

    public SolutionsController(SolutionService solutionService)
    {
        _solutionService = solutionService;
    }

    // POST /api/challenges/{challengeId}/solutions
    [Authorize]
    [HttpPost("challenges/{challengeId}/solutions")]
    public async Task<IActionResult> Submit(Guid challengeId, [FromBody] SubmitSolutionRequest request)
    {
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
        try
        {
            var userId = GetUserId();
            var solution = await _solutionService.ReviewAsync(id, request, userId);
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