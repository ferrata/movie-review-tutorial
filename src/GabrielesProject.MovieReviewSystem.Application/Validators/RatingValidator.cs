using FluentValidation;
using GabrielesProject.MovieReviewSystem.Application.Interfaces;

namespace GabrielesProject.MovieReviewSystem.Application.Validators;

internal class RatingValidator : IRatingValidator
{
    public void ValidateAndThrow(int rating)
    {
        var validator = new RatingValidatorRules();
        validator.ValidateAndThrow(rating);
    }

    private class RatingValidatorRules : AbstractValidator<int>
    {
        public RatingValidatorRules() =>
            RuleFor(x => x)
                .InclusiveBetween(1, 5)
                .WithMessage("Invalid rating value, must be between 1 and 5");
    }
}