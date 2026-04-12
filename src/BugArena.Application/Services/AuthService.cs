using BugArena.Application.DTOs.Users;
using BugArena.Application.Interfaces;
using BugArena.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BugArena.Application.Services;

public class AuthService
{
    // Service for handling user authentication, including registration, login, and profile retrieval.
    private readonly IUserRepository _userRepository;
    
    // Unit of work for managing database transactions.
    private readonly IUnitOfWork _unitOfWork;
    
    // Service for generating JWT tokens for authenticated users.
    private readonly ITokenService _tokenService;
    
    // Service for hashing and verifying user passwords.
    private readonly IPasswordHasher<User> _passwordHasher;

    // Constructor to initialize the AuthService with required dependencies.
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

    // Registers a new user and returns an authentication response with a JWT token.
    public async Task<AuthResponse> RegisterAsync(RegisterRequestDtos request)
    {
        if (await _userRepository.GetByEmailAsync(request.Email) is not null)
            throw new InvalidOperationException("Email is already in use.");

        if (await _userRepository.GetByUsernameAsync(request.Username) is not null)
            throw new InvalidOperationException("Username is already taken.");

        // Create a new user entity with the provided registration details.
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
        };

        // Hash the user's password before storing it in the database.
        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        // Generate a JWT token for the newly registered user and return the authentication response.
        return new AuthResponse
        {
            Id = user.Id,
            Token = _tokenService.GenerateToken(user),
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        };
    }

    // Authenticates a user based on their email and password, returning an authentication response with a JWT token if successful.
    public async Task<AuthResponse> LoginAsync(LoginRequestDtos request)
    {
        // Retrieve the user by email from the database. If the user does not exist, throw an unauthorized access exception.
        var user = await _userRepository.GetByEmailAsync(request.Email)
            ?? throw new UnauthorizedAccessException("Invalid email or password.");

        // Verify the provided password against the stored password hash. If the verification fails, throw an unauthorized access exception.
        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
            throw new UnauthorizedAccessException("Invalid email or password.");

        // Generate a JWT token for the authenticated user and return the authentication response.
        return new AuthResponse
        {
            Id = user.Id,
            Token = _tokenService.GenerateToken(user),
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        };
    }

    // Retrieves the profile information of the currently authenticated user based on their user ID.
    public async Task<UserSummaryResponse> GetMeAsync(Guid userId)
    {
        //  Retrieve the user by ID from the database. If the user does not exist, throw an unauthorized access exception.
        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new UnauthorizedAccessException("User not found.");

        // Map the user entity to a UserSummaryResponse DTO and return it.
        return new UserSummaryResponse(
            
            user.Id,
            user.Username,
            user.Email,
            user.Role,
            user.TotalPoints,
            user.AvatarUrl,
            user.CreatedAt
        );
    }
}