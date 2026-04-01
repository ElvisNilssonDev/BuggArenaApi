using BugArena.Application.Interfaces;
using BugArena.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BugArena.Infrastructure.Services;

public sealed class PasswordService : IPasswordService
{
    private readonly IPasswordHasher<User> _passwordHasher;

    public PasswordService(IPasswordHasher<User> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public string HashPassword(User user, string password)
        => _passwordHasher.HashPassword(user, password);

    public bool VerifyPassword(User user, string hashedPassword, string providedPassword)
        => _passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword)
           != PasswordVerificationResult.Failure;
}