using System.Security.Claims;
using BugArena.Application.DTOs.Users;
using BugArena.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BugArena.API.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AdminController(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<UserSummaryResponse>>> GetAllUsers()
    {
        var users = await _userRepository.GetAllAsync();

        var response = users.Select(user => new UserSummaryResponse(
            user.Id,
            user.Username,
            user.Email,
            user.Role,
            user.TotalPoints,
            user.AvatarUrl,
            user.CreatedAt
        ));

        return Ok(response);
    }

    [HttpPatch("users/{id:guid}/role")]
    public async Task<IActionResult> UpdateRole(Guid id, [FromBody] UpdateRoleRequest request)
    {
        if (request is null)
            return BadRequest("Request body is required.");

        var currentUserIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (currentUserIdClaim is null || !Guid.TryParse(currentUserIdClaim, out var currentUserId))
            return Unauthorized("Invalid token: missing user id.");

        if (id == currentUserId && request.Role == "User")
            return BadRequest("You cannot remove your own admin role.");

        var updated = await _userRepository.UpdateRoleAsync(id, request.Role);

        if (!updated)
            return NotFound();

        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("users/{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var currentUserIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (currentUserIdClaim is null || !Guid.TryParse(currentUserIdClaim, out var currentUserId))
            return Unauthorized("Invalid token: missing user id.");

        if (id == currentUserId)
            return BadRequest("You cannot delete your own account.");

        var deleted = await _userRepository.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }
}