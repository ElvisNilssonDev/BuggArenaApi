using BugArena.Domain.Entities;
using BugArena.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BugArena.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Challenge> Challenges => Set<Challenge>();
    public DbSet<Solution> Solutions => Set<Solution>();
    public DbSet<Vote> Votes => Set<Vote>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).ValueGeneratedOnAdd();
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasIndex(u => u.Username).IsUnique();
            entity.Property(u => u.Email).IsRequired().HasMaxLength(256);
            entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.Role).IsRequired().HasMaxLength(20);
            entity.Property(u => u.AvatarUrl).HasMaxLength(512);
        });

        modelBuilder.Entity<Challenge>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Title).IsRequired().HasMaxLength(200);
            e.Property(c => c.BuggyCode).IsRequired();
            e.Property(c => c.Language).HasConversion<string>();
            e.Property(c => c.Difficulty).HasConversion<string>();
            e.Property(c => c.Status).HasConversion<string>();

            e.HasOne(c => c.Author)
             .WithMany(u => u.AuthoredChallenges)
             .HasForeignKey(c => c.AuthorId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Solution>(e =>
        {
            e.HasKey(s => s.Id);
            e.Property(s => s.Status).HasConversion<string>();

            e.HasOne(s => s.Solver)
             .WithMany(u => u.Solutions)
             .HasForeignKey(s => s.SolverId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(s => s.Challenge)
             .WithMany(c => c.Solutions)
             .HasForeignKey(s => s.ChallengeId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Vote>(e =>
        {
            e.HasKey(v => v.Id);
            e.HasIndex(v => new { v.UserId, v.ChallengeId }).IsUnique();

            e.HasOne(v => v.User)
             .WithMany(u => u.Votes)
             .HasForeignKey(v => v.UserId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(v => v.Challenge)
             .WithMany(c => c.Votes)
             .HasForeignKey(v => v.ChallengeId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}