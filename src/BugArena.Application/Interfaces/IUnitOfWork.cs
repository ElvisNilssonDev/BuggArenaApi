namespace BugArena.Application.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}