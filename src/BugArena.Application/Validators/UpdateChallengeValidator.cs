using BugArena.Application.DTOs.Challenges;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BugArena.Application.Validators
{
    public class UpdateChallengeValidator : AbstractValidator<UpdateChallengeRequest>
    {
        public UpdateChallengeValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(5000);

            RuleFor(x => x.ExpectedBehavior)
                .NotEmpty().WithMessage("Expected behavior is required.")
                .MaximumLength(2000);

            RuleFor(x => x.ActualBehavior)
                .NotEmpty().WithMessage("Actual behavior is required.")
                .MaximumLength(2000);

            RuleFor(x => x.Hints)
                .MaximumLength(2000).When(x => x.Hints != null);

            RuleFor(x => x.Difficulty)
                .IsInEnum().WithMessage("Invalid difficulty.");
        }
    }
}
