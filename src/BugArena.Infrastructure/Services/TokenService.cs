using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BugArena.Application.Interfaces;
using BugArena.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BugArena.Infrastructure.Services;

// TokenService is responsible for generating JWT tokens for authenticated users. It implements the ITokenService interface and uses the JwtSettings configuration to create secure tokens that include user claims such as ID, email, username, and role. The generated token can be used for authenticating API requests and authorizing access to protected resources.
public class TokenService : ITokenService
{
    // JWT settings for token generation, injected via IOptions<JwtSettings>.
    private readonly JwtSettings _jwt;

    // Constructor to initialize the TokenService with the provided JWT settings, allowing it to generate tokens based on the configured parameters such as key, issuer, audience, and expiry time.
    public TokenService(IOptions<JwtSettings> jwt)
    {
        _jwt = jwt.Value;
    }

    // Generates a JWT token for the specified user, including claims for user ID, email, username, and role. The token is signed using the configured key and includes an expiration time based on the JwtSettings configuration.
    public string GenerateToken(User user)
    {
        // Define the claims to be included in the JWT token, such as user ID, email, username, role, and a unique identifier (JTI) for the token.
        var claims = new[]
        {
            // Standard JWT claims for user identification and authentication.
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),

            // Standard JWT claim for the user's email address.
            new Claim(JwtRegisteredClaimNames.Email, user.Email),

            // Standard JWT claim for the user's username.
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),

            // Custom claim for the user's role, which can be used for authorization purposes.
            new Claim("role", user.Role),

            // Standard JWT claim for a unique identifier (JTI) to prevent token reuse.
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Create a symmetric security key using the configured JWT key and encode it as UTF-8 bytes.
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));

        // Create signing credentials using the security key and specify the HMAC SHA256 algorithm for signing the token.
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Create a new JWT token with the specified issuer, audience, claims, expiration time, and signing credentials. The token will be valid for a duration defined in the JwtSettings configuration.
        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.ExpiryMinutes),
            signingCredentials: creds);

        // Serialize the JWT token to a string format that can be returned to the client for use in authentication and authorization.
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}