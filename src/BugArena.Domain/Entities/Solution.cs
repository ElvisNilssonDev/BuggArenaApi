using BugArena.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BugArena.Domain.Entities
{
    public class Solution
    {
        public Guid Id { get; set; }
        public Guid SolverId { get; set; }
        public Guid ChallengeId { get; set; }
        public string FixedCode { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;
        public SolutionStatus Status { get; set; } = SolutionStatus.Pending;
        public int PointsAwarded { get; set; } = 0;
        public int AttemptNumber { get; set; }
        public int TimeToSolveSeconds { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public User Solver { get; set; } = null!;
        public Challenge Challenge { get; set; } = null!;
    }
}
