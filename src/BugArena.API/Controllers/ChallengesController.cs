using BugArena.Application.DTOs.Challenges;
using BugArena.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BugArena.API.Controllers;

[ApiController]
[Route("api/challenges")]
public class ChallengesController : ControllerBase
{
    private readonly ChallengeService _challengeService;
    private readonly IValidator<CreateChallengeRequest> _createValidator;
    private readonly IValidator<UpdateChallengeRequest> _updateValidator;

    public ChallengesController(
        ChallengeService challengeService,
        IValidator<CreateChallengeRequest> createValidator,
        IValidator<UpdateChallengeRequest> updateValidator)
    {
        _challengeService = challengeService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
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
        var validation = await _createValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return BadRequest(validation.Errors);

        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _challengeService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // PUT /api/challenges/{id}
    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateChallengeRequest dto)
    {
        var validation = await _updateValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return BadRequest(validation.Errors);

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
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "User";
        var deleted = await _challengeService.DeleteAsync(id, userId, role);
        return deleted ? NoContent() : NotFound();
    }

    // PATCH /api/challenges/{id}/close
    [HttpPatch("{id:guid}/close")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Close(Guid id)
    {
        var closed = await _challengeService.CloseAsync(id);
        return closed ? NoContent() : NotFound();
    }
}