using BugArena.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BugArena.Infrastructure.Data.Configurations;

public class VoteConfiguration : IEntityTypeConfiguration<Vote>
{
    public void Configure(EntityTypeBuilder<Vote> builder)
    {
        builder.HasKey(v => v.Id);

        builder.HasOne(v => v.User)
            .WithMany(u => u.Votes)
            .HasForeignKey(v => v.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(v => v.Challenge)
            .WithMany(c => c.Votes)
            .HasForeignKey(v => v.ChallengeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(v => new { v.UserId, v.ChallengeId })
            .IsUnique();
    }
}