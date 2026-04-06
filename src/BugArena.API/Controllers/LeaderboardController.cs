using BugArena.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BugArena.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaderboardController : ControllerBase
{
    private readonly LeaderboardService _leaderboardService;

    public LeaderboardController(LeaderboardService leaderboardService)
    {
        _leaderboardService = leaderboardService;
    }

    // GET /api/leaderboard/global
    [HttpGet("global")]
    public async Task<IActionResult> GetGlobal()
    {
        var entries = await _leaderboardService.GetGlobalAsync();
        return Ok(entries);
    }

    // GET /api/leaderboard/weekly
    [HttpGet("weekly")]
    public async Task<IActionResult> GetWeekly()
    {
        var entries = await _leaderboardService.GetWeeklyAsync();
        return Ok(entries);
    }

    // GET /api/leaderboard/user/{id}
    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetUserStats(Guid id)
    {
        try
        {
            var stats = await _leaderboardService.GetUserStatsAsync(id);
            return Ok(stats);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
