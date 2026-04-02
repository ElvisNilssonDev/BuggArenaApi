using BugArena.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BugArena.Infrastructure.Data.Configurations;

public class ChallengeConfiguration : IEntityTypeConfiguration<Challenge>
{
    public void Configure(EntityTypeBuilder<Challenge> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(5000);

        builder.Property(c => c.BuggyCode)
            .IsRequired()
            .HasMaxLength(50000);

        builder.Property(c => c.ExpectedBehavior)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(c => c.ActualBehavior)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(c => c.Hints)
            .HasMaxLength(2000);

        builder.Property(c => c.Language)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(c => c.Difficulty)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(10);

        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasOne(c => c.Author)
            .WithMany(u => u.AuthoredChallenges)
            .HasForeignKey(c => c.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => c.Status);
        builder.HasIndex(c => c.Language);
        builder.HasIndex(c => c.Difficulty);
        builder.HasIndex(c => c.CreatedAt);
    }
}