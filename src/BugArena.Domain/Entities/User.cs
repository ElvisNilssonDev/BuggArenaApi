namespace BugArena.Domain.Entities;

// Represents a user in the BugArena system.
public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public int TotalPoints { get; set; } = 0;
    public string Role { get; set; } = "User"; // User, Moderator, Admin
    public string? AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Challenge> AuthoredChallenges { get; set; } = new List<Challenge>();
    public ICollection<Solution> Solutions { get; set; } = new List<Solution>();
    public ICollection<Vote> Votes { get; set; } = new List<Vote>();
}