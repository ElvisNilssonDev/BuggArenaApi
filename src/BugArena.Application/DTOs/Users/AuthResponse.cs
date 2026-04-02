namespace BugArena.Application.DTOs.Users;

// DTO for returning authentication response data, including the JWT token and user information.
public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}