using FluentValidation;
using GabrielesProject.MovieReviewSystem.Application.DTOs;
using GabrielesProject.MovieReviewSystem.Application.Interfaces;
using Model = GabrielesProject.MovieReviewSystem.Domain.Entities;

namespace GabrielesProject.MovieReviewSystem.Application.Services;

public interface IMovieService
{
    Task<IEnumerable<Movie>> GetMoviesAsync();
    Task<IEnumerable<Movie>> GetMoviesAsync(int? minRating, int? maxRating);
    Task<Movie> GetMovieAsync(int id);
    Task<Movie> AddMovieAsync(CreateMovieArgs movie);
    Task DeleteMovieAsync(int id);
    Task RateMovieAsync(int id, int rating);
}

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IMovieRatingRepository _movieRatingRepository;
    private readonly ICommentsService _commentsService;
    private readonly IRatingValidator _ratingValidator;

    public MovieService(
        IMovieRepository movieRepository,
        IMovieRatingRepository movieRatingRepository,
        ICommentsService commentsService,
        IRatingValidator ratingValidator)
    {
        _movieRepository = movieRepository;
        _movieRatingRepository = movieRatingRepository;
        _commentsService = commentsService;
        _ratingValidator = ratingValidator;
    }

    public async Task<Movie> AddMovieAsync(CreateMovieArgs movie)
    {
        var movieId = await _movieRepository.AddMovieAsync(new Model.Movie { Title = movie.Title, Summary = movie.Summary });
        return await GetMovieAsync(movieId);
    }

    public Task DeleteMovieAsync(int id)
    {
        return _movieRepository.DeleteMovieAsync(id);
    }

    public async Task<IEnumerable<Movie>> GetMoviesAsync()
    {
        var moviesFromDb = await _movieRepository.GetMoviesAsync();
        return await ConvertToDto(moviesFromDb);
    }

    public async Task<IEnumerable<Movie>> GetMoviesAsync(int? minRating, int? maxRating)
    {
        if (minRating is not null && maxRating is not null)
        { 
            _ratingValidator.ValidateAndThrow(minRating.Value);
            _ratingValidator.ValidateAndThrow(maxRating.Value);
        }

        var moviesFromDb = await _movieRepository.GetMoviesAsync(minRating ?? 0, maxRating ?? 5);
        var movies = await ConvertToDto(moviesFromDb);
        return movies.Where(m => m.Rating >= minRating && m.Rating <= maxRating);
    }

    public async Task<Movie> GetMovieAsync(int id)
    {
        var movieFromDb = await _movieRepository.GetMovieAsync(id);
        if (movieFromDb is null)
        {
            throw new KeyNotFoundException();
        }

        Movie movie = await ConvertToDto(movieFromDb);

        return movie;
    }

    public Task RateMovieAsync(int id, int rating)
    {
        _ratingValidator.ValidateAndThrow(rating);
        return _movieRatingRepository.AddRatingAsync(id, rating);
    }

    private async Task<decimal?> GetAverageRatings(int movieId)
    {
        var ratings = (await _movieRatingRepository.GetRatingsAsync(movieId)).ToList();
        if (ratings.Count > 0)
        {
            return (decimal)ratings.Average();
        }

        return null;
    }

    private async Task<List<Comment>> GetFiveComments(int movieId)
    {
        var externalComments = await _commentsService.GetCommentsAsync(movieId);
        return externalComments
            .Take(5)
            .Select(c => new Comment { Id = c.Id, Name = c.Name, Text = c.Body })
            .ToList();
    }

    private async Task<IEnumerable<Movie>> ConvertToDto(IEnumerable<Model.Movie> moviesFromDb)
    {
        var result = new List<Movie>();
        foreach (var movieFromDb in moviesFromDb)
        {
            var movie = await ConvertToDto(movieFromDb);
            result.Add(movie);
        }

        return result;
    }

    private async Task<Movie> ConvertToDto(Model.Movie movieFromDb)
    {
        var movie = new Movie { Id = movieFromDb.Id, Title = movieFromDb.Title, Summary = movieFromDb.Summary };
        movie.Rating = await GetAverageRatings(movie.Id);
        movie.Comments = await GetFiveComments(movie.Id);
        return movie;
    }
}