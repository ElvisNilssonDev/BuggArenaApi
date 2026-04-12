namespace BugArena.Application.DTOs.Users;

// DTO for user login requests, containing the email and password fields.
public class LoginRequestDtos
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}