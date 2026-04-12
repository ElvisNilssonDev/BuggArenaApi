namespace BugArena.Application.DTOs.Users;

// DTO for user registration requests, containing the username, email, and password fields.
public class RegisterRequestDtos
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}