using BugArena.Application.DTOs.Challenges;
using BugArena.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BugArena.API.Controllers;

[ApiController]
[Route("api/challenges")]
public class ChallengesController : ControllerBase
{
    private readonly ChallengeService _challengeService;

    public ChallengesController(ChallengeService challengeService)
    {
        _challengeService = challengeService;
    }

    // GET /api/challenges?language=CSharp&difficulty=Hard&page=1&pageSize=10
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ChallengeQueryParams queryParams)
    {
        var result = await _challengeService.GetAllAsync(queryParams);
        return Ok(result);
    }

    // GET /api/challenges/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _challengeService.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    // POST /api/challenges
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateChallengeRequest dto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _challengeService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // PUT /api/challenges/{id}
    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateChallengeRequest dto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var updated = await _challengeService.UpdateAsync(id, dto, userId);
        return updated ? NoContent() : NotFound();
    }

    // DELETE /api/challenges/{id}
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var deleted = await _challengeService.DeleteAsync(id, userId);
        return deleted ? NoContent() : NotFound();
    }
}