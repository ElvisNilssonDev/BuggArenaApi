using BugArena.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BugArena.Domain.Entities
{
    public class Challenge
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string BuggyCode { get; set; } = string.Empty;
        public string ExpectedBehavior { get; set; } = string.Empty;
        public string ActualBehavior { get; set; } = string.Empty;
        public string? Hints { get; set; }
        public Language Language { get; set; }
        public Difficulty Difficulty { get; set; }
        public ChallengeStatus Status { get; set; } = ChallengeStatus.Published;
        public int MaxPoints { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ClosedAt { get; set; }

        // Navigation
        public User Author { get; set; } = null!;
        public ICollection<Solution> Solutions { get; set; } = new List<Solution>();
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
    }
}
