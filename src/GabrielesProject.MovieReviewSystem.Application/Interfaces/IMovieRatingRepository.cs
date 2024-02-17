namespace GabrielesProject.MovieReviewSystem.Application.Interfaces;

public interface IMovieRatingRepository
{
    Task<IEnumerable<int>> GetRatingsAsync(int movieId);
    Task AddRatingAsync(int movieId, int rating);
}
