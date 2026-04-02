using BugArena.Application.DTOs.Users;
using BugArena.Application.Interfaces;
using BugArena.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BugArena.Application.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthService(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ITokenService tokenService,
        IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _userRepository.GetByEmailAsync(request.Email) is not null)
            throw new InvalidOperationException("Email is already in use.");

        if (await _userRepository.GetByUsernameAsync(request.Username) is not null)
            throw new InvalidOperationException("Username is already taken.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return new AuthResponse
        {
            Token = _tokenService.GenerateToken(user),
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email)
            ?? throw new UnauthorizedAccessException("Invalid email or password.");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
            throw new UnauthorizedAccessException("Invalid email or password.");

        return new AuthResponse
        {
            Token = _tokenService.GenerateToken(user),
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        };
    }

    public async Task<UserProfileResponse> GetMeAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new UnauthorizedAccessException("User not found.");

        return new UserProfileResponse
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            TotalPoints = user.TotalPoints,
            AvatarUrl = user.AvatarUrl,
            CreatedAt = user.CreatedAt
        };
    }
}