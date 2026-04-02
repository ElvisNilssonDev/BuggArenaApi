using BugArena.Application.Interfaces;
using BugArena.Domain.Entities;
using BugArena.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BugArena.Infrastructure.Repositories;

// UserRepository is responsible for managing user data in the database. It implements the IUserRepository interface and provides methods for retrieving users by email, username, and ID, as well as adding new users to the database. The repository uses Entity Framework Core to interact with the AppDbContext and perform database operations asynchronously.
public class UserRepository : IUserRepository
{
    // The AppDbContext instance used to interact with the database.
    private readonly AppDbContext _context;

    // Constructor to initialize the UserRepository with the provided AppDbContext, allowing it to perform database operations related to user data.
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    // Retrieves a user from the database based on their email address asynchronously. It returns the user if found, or null if no user with the specified email exists.
    public Task<User?> GetByEmailAsync(string email) =>

        // Uses Entity Framework Core to query the Users DbSet for a user with the specified email. The FirstOrDefaultAsync method is used to return the first matching user or null if no match is found.
        _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    // Retrieves a user from the database based on their username asynchronously. It returns the user if found, or null if no user with the specified username exists.
    public Task<User?> GetByUsernameAsync(string username) =>
        
        // Uses Entity Framework Core to query the Users DbSet for a user with the specified username. The FirstOrDefaultAsync method is used to return the first matching user or null if no match is found.
        _context.Users.FirstOrDefaultAsync(u => u.Username == username);

    // Adds a new user to the database asynchronously. The method takes a User object as a parameter and adds it to the Users DbSet using the AddAsync method provided by Entity Framework Core.
    public async Task AddAsync(User user) =>

        // Uses Entity Framework Core to add the provided User object to the Users DbSet. The AddAsync method is used to perform the operation asynchronously, allowing for efficient database interactions without blocking the calling thread.
        await _context.Users.AddAsync(user);

    // Retrieves a user from the database based on their unique identifier (ID) asynchronously. It returns the user if found, or null if no user with the specified ID exists.
    public Task<User?> GetByIdAsync(Guid id) =>

    // Uses Entity Framework Core to query the Users DbSet for a user with the specified ID. The FirstOrDefaultAsync method is used to return the first matching user or null if no match is found..
    _context.Users.FirstOrDefaultAsync(u => u.Id == id);
}