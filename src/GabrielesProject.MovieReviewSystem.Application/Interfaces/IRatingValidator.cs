using FluentValidation;

namespace GabrielesProject.MovieReviewSystem.Application.Interfaces;

public interface IRatingValidator
{
    void ValidateAndThrow(int rating);
}