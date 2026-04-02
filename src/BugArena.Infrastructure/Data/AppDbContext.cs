using BugArena.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BugArena.Infrastructure.Data;

// AppDbContext is the Entity Framework Core database context for the BugArena application. It manages the connection to the database and provides DbSet properties for each entity type, allowing for querying and saving data. The OnModelCreating method configures the entity mappings and constraints for the User entity.
public class AppDbContext : DbContext
{
    // Constructor that accepts DbContextOptions and passes them to the base DbContext constructor.
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // DbSet property for the User entity, allowing for querying and saving User data in the database.
    public DbSet<User> Users => Set<User>();

    // Configures the entity mappings and constraints for the User entity using the Fluent API.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Call the base implementation of OnModelCreating to ensure that any configurations defined in the base class are applied.
        modelBuilder.Entity<User>(entity =>
        {
            // Configure the primary key for the User entity and set it to be generated on add.
            entity.HasKey(u => u.Id);

            // Configure the Id property to be generated on add, meaning it will be automatically assigned a value when a new User is added to the database.
            entity.Property(u => u.Id).ValueGeneratedOnAdd();

            // Configure unique indexes on the Email and Username properties to ensure that each email and username is unique in the database.
            entity.HasIndex(u => u.Email).IsUnique();

            // Configure a unique index on the Username property to ensure that each username is unique in the database.
            entity.HasIndex(u => u.Username).IsUnique();

            // Configure the Email property to be required and have a maximum length of 256 characters.
            entity.Property(u => u.Email).IsRequired().HasMaxLength(256);

            // Configure the Username property to be required and have a maximum length of 50 characters.
            entity.Property(u => u.Username).IsRequired().HasMaxLength(50);

            // Configure the PasswordHash property to be required, ensuring that a password hash is always stored for each user.
            entity.Property(u => u.PasswordHash).IsRequired();

            // Configure the Role property to be required and have a maximum length of 20 characters, defining the user's role in the application.
            entity.Property(u => u.Role).IsRequired().HasMaxLength(20);

            // Configure the AvatarUrl property to have a maximum length of 512 characters, allowing for storing URLs to user avatars.
            entity.Property(u => u.AvatarUrl).HasMaxLength(512);
        });
    }
}