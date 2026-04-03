using System;
using System.Collections.Generic;
using System.Text;

namespace BugArena.Domain.Entities
{
    public class Vote
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ChallengeId { get; set; }
        public bool IsUpvote { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public User User { get; set; } = null!;
        public Challenge Challenge { get; set; } = null!;
    }
}
