using System.Security.Claims;
using BugArena.Application.DTOs.Users;
using BugArena.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BugArena.API.Controllers;

// / Controller for handling user authentication and profile retrieval.
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{

    // Dependency injection of the AuthService to handle authentication logic.
    private readonly AuthService _authService;

    // Constructor to initialize the AuthService.
    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    // Endpoint for user registration. Accepts a RegisterRequest and returns an AuthResponse.
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var response = await _authService.RegisterAsync(request);
        return Ok(response);
    }

    // Endpoint for user login. Accepts a LoginRequest and returns an AuthResponse.
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        return Ok(response);
    }

    // Endpoint to retrieve the authenticated user's profile information. Requires authorization.
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");

        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var response = await _authService.GetMeAsync(userId);
        return Ok(response);
    }
}