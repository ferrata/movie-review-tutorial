using FluentValidation;
using GabrielesProject.MovieReviewSystem.Application.Interfaces;
using GabrielesProject.MovieReviewSystem.Domain.Exceptions;

namespace GabrielesProject.MovieReviewSystem.Application.Validators;

internal class RatingValidator : IRatingValidator
{
    public void ValidateAndThrow(int rating)
    {
        var validator = new RatingValidatorRules();
        var result = validator.Validate(rating);
        if (result.IsValid)
        {
            return;
        }
        
        var errorMessage = string.Join(" - ", result.Errors.Select(x => x.ErrorMessage));
        throw new InvalidInputException(errorMessage);
    }

    private class RatingValidatorRules : AbstractValidator<int>
    {
        public RatingValidatorRules() =>
            RuleFor(x => x)
                .InclusiveBetween(1, 5)
                .WithMessage("Invalid rating value, must be between 1 and 5");
    }
}