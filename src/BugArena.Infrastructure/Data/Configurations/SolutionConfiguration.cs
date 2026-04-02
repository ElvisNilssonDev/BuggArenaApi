using BugArena.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BugArena.Infrastructure.Data.Configurations;

public class SolutionConfiguration : IEntityTypeConfiguration<Solution>
{
    public void Configure(EntityTypeBuilder<Solution> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.FixedCode)
            .IsRequired()
            .HasMaxLength(50000);

        builder.Property(s => s.Explanation)
            .IsRequired()
            .HasMaxLength(5000);

        builder.Property(s => s.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(s => s.Feedback)
            .HasMaxLength(2000);

        builder.HasOne(s => s.Solver)
            .WithMany(u => u.Solutions)
            .HasForeignKey(s => s.SolverId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Challenge)
            .WithMany(c => c.Solutions)
            .HasForeignKey(s => s.ChallengeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => s.ChallengeId);
        builder.HasIndex(s => s.SolverId);
        builder.HasIndex(s => s.Status);
        builder.HasIndex(s => new { s.ChallengeId, s.SolverId });
    }
}