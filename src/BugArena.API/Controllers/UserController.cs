using System.Security.Claims;
using BugArena.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BugArena.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ProfileService _profileService;

    public UsersController(ProfileService profileService)
    {
        _profileService = profileService;
    }

    // GET /api/users/{id}/profile
    [HttpGet("{id}/profile")]
    public async Task<IActionResult> GetProfile(Guid id)
    {
        try
        {
            var profile = await _profileService.GetProfileAsync(id);
            return Ok(profile);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // GET /api/users/{id}/challenges
    [HttpGet("{id}/challenges")]
    public async Task<IActionResult> GetUserChallenges(Guid id, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _profileService.GetUserChallengesAsync(id, page, pageSize);
        return Ok(result);
    }

    // GET /api/users/{id}/solutions
    [Authorize]
    [HttpGet("{id}/solutions")]
    public async Task<IActionResult> GetUserSolutions(Guid id, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        // Only the user themselves can see their solution history
        var userId = GetUserId();
        if (userId != id)
            return StatusCode(403, new { message = "You can only view your own solution history." });

        var result = await _profileService.GetUserSolutionsAsync(id, page, pageSize);
        return Ok(result);
    }

    private Guid GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User ID not found in token.");
        return Guid.Parse(claim.Value);
    }
}
