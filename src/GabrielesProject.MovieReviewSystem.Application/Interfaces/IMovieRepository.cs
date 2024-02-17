using Model = GabrielesProject.MovieReviewSystem.Domain.Entities;

namespace GabrielesProject.MovieReviewSystem.Application.Interfaces;

public interface IMovieRepository
{
    Task<IEnumerable<Model.Movie>> GetMoviesAsync();
    Task<IEnumerable<Model.Movie>> GetMoviesAsync(int minRating, int maxRating);
    Task<Model.Movie?> GetMovieAsync(int id);
    Task<int> AddMovieAsync(Model.Movie movie);
    Task DeleteMovieAsync(int id);
}
