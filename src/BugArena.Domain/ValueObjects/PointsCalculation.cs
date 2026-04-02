using BugArena.Domain.Enums;

namespace BugArena.Domain.ValueObjects;

public static class PointsCalculation
{
    public static int GetBasePoints(Difficulty difficulty) => difficulty switch
    {
        Difficulty.Easy => 100,
        Difficulty.Medium => 250,
        Difficulty.Hard => 500,
        _ => 100
    };

    public static int Calculate(
        Difficulty difficulty,
        bool isFirstSolver,
        int attemptNumber,
        int timeToSolveSeconds)
    {
        int basePoints = GetBasePoints(difficulty);
        double multiplier = 1.0;

        if (isFirstSolver)
            multiplier *= 2.0;

        if (attemptNumber == 1)
            multiplier *= 1.5;

        if (timeToSolveSeconds < 300)
            multiplier *= 1.3;
        else if (timeToSolveSeconds < 900)
            multiplier *= 1.1;

        int penalty = (attemptNumber - 1) * (int)(basePoints * 0.10);
        int finalPoints = (int)(basePoints * multiplier) - penalty;

        return Math.Max(finalPoints, 10);
    }
}