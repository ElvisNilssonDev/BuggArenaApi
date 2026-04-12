using BugArena.Application.Interfaces;

namespace BugArena.Infrastructure.Data;

// Implements the Unit of Work pattern to manage database transactions and ensure that all operations are completed successfully before committing changes to the database.
public class UnitOfWork : IUnitOfWork
{
    // The database context used to interact with the database and manage entity states.
    private readonly AppDbContext _context;

    // Constructor to initialize the UnitOfWork with the provided AppDbContext, allowing it to manage database operations and transactions.
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    // Saves all changes made in the context to the database asynchronously, ensuring that all operations are completed successfully before committing changes.
    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}