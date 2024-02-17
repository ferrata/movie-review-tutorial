using System.Data;
using Dapper;
using GabrielesProject.MovieReviewSystem.Application.Interfaces;
using Model = GabrielesProject.MovieReviewSystem.Domain.Entities;

namespace GabrielesProject.MovieReviewSystem.Infrastracture.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly IDbConnection _dbConnection;

    public MovieRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public Task<int> AddMovieAsync(Model.Movie movie)
    {
        return _dbConnection.ExecuteScalarAsync<int>("INSERT INTO movies (title, summary) VALUES (@Title, @Summary) RETURNING id", movie);
    }

    public Task<Model.Movie?> GetMovieAsync(int id)
    {
        return _dbConnection.QueryFirstOrDefaultAsync<Model.Movie>("SELECT * FROM movies WHERE id = @Id", new { Id = id });
    }

    public Task DeleteMovieAsync(int id)
    {
        return _dbConnection.ExecuteAsync("DELETE FROM movies WHERE id = @Id", new { Id = id });
    }

    public Task<IEnumerable<Model.Movie>> GetMoviesAsync()
    {
        return _dbConnection.QueryAsync<Model.Movie>("SELECT * FROM movies");
    }

    public Task<IEnumerable<Model.Movie>> GetMoviesAsync(int minRating, int maxRating)
    {
        return _dbConnection.QueryAsync<Model.Movie>("SELECT m.* FROM movies m JOIN ratings r ON m.id = r.movie_id WHERE r.rating >= @MinRating AND r.rating <= @MaxRating", new { MinRating = minRating, MaxRating = maxRating });
    }
}
