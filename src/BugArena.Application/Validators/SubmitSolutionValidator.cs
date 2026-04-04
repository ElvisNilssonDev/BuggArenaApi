using BugArena.Application.DTOs.Solutions;
using FluentValidation;

namespace BugArena.Application.Validators;

public class SubmitSolutionValidator : AbstractValidator<SubmitSolutionRequest>
{
    public SubmitSolutionValidator()
    {
        RuleFor(x => x.FixedCode)
            .NotEmpty().WithMessage("Fixed code is required.")
            .MaximumLength(50000);

        RuleFor(x => x.Explanation)
            .NotEmpty().WithMessage("Explanation is required.")
            .MaximumLength(5000);

        RuleFor(x => x.TimeToSolveSeconds)
            .GreaterThan(0).WithMessage("Solve time must be positive.");
    }
}