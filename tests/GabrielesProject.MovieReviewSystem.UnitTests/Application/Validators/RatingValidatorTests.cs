using FluentAssertions;
using FluentValidation;
using GabrielesProject.MovieReviewSystem.Application.Validators;

namespace GabrielesProject.MovieReviewSystem.ServiceTests.Application.Validators;

public class RatingValidatorTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void WhenValidRating_ThenDoesNotThrow(int rating)
    {
        // Arrange
        var ratingValidator = new RatingValidator();
        
        // Act & Assert
        ratingValidator
            .Invoking(x => x.ValidateAndThrow(rating))
            .Should().NotThrow();
    }
    
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    public void WhenInvalidRating_ThenThrows(int rating)
    {
        // Arrange
        var ratingValidator = new RatingValidator();
        
        // Act & Assert
        ratingValidator
            .Invoking(x => x.ValidateAndThrow(rating))
            .Should().Throw<ValidationException>()
            .WithMessage("*Invalid rating value, must be between 1 and 5*");
    }
}