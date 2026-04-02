namespace BugArena.Application.DTOs.Users;

// DTO for returning user profile information, including the user's ID, username, email, role, total points, avatar URL, and account creation date.
public class UserProfileResponse
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int TotalPoints { get; set; }
    public string? AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}