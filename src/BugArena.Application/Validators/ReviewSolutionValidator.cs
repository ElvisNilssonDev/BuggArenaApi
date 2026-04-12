using BugArena.Application.DTOs.Solutions;
using BugArena.Domain.Enums;
using FluentValidation;

namespace BugArena.Application.Validators;

public class ReviewSolutionValidator : AbstractValidator<ReviewSolutionRequest>
{
    public ReviewSolutionValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum()
            .Must(s => s == SolutionStatus.Approved || s == SolutionStatus.Rejected)
            .WithMessage("Status must be Approved or Rejected.");

        RuleFor(x => x.Feedback)
            .MaximumLength(2000).When(x => x.Feedback != null);
    }
}