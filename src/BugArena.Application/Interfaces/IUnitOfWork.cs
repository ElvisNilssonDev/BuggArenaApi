namespace BugArena.Application.Interfaces;

// Unit of Work pattern to manage database transactions and ensure that all operations either succeed or fail together.
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}