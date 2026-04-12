namespace BugArena.Infrastructure;

// JwtSettings is a configuration class that holds the settings required for generating JWT tokens. It includes properties for the secret key, issuer, audience, and token expiry time. This class is used to bind the JWT settings from the application's configuration file (e.g., appsettings.json) and provides a structured way to access these settings throughout the application when generating tokens for authentication and authorization purposes.
public sealed class JwtSettings
{
    public const string SectionName = "Jwt";
    public string Key { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public int ExpiryMinutes { get; init; } = 60;
}