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

    // GET /api/admin/users
    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userRepository.GetAllAsync();
        return Ok(users);
    }

    // PATCH /api/admin/users/{id}/role
    [HttpPatch("users/{id:guid}/role")]
    public async Task<IActionResult> UpdateRole(Guid id, [FromBody] UpdateRoleRequest request)
    {
        var updated = await _userRepository.UpdateRoleAsync(id, request.Role);
        return updated ? NoContent() : NotFound();
    }

    // DELETE /api/admin/users/{id}
    [HttpDelete("users/{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var deleted = await _userRepository.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}