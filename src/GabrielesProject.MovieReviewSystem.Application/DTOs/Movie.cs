namespace GabrielesProject.MovieReviewSystem.Application.DTOs;

public record Comment
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Text { get; set; }
}

public record Movie
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Summary { get; set; }

    public decimal? Rating { get; set; }

    public List<Comment>? Comments { get; set; }
}

public record CreateMovieArgs
{
    public string? Title { get; set; }

    public string? Summary { get; set; }
}
