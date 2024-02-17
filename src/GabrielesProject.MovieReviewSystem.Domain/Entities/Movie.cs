namespace GabrielesProject.MovieReviewSystem.Domain.Entities;

public record Movie
{
    public int Id { get; init; }
    public string? Title { get; init; }
    public string? Summary { get; init; }
}
