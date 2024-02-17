using System.Data;
using Dapper;
using GabrielesProject.MovieReviewSystem.Application.Interfaces;

namespace GabrielesProject.MovieReviewSystem.Infrastracture.Repositories;

public class MovieRatingRepository : IMovieRatingRepository
{
    private readonly IDbConnection _dbConnection;

    public MovieRatingRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public Task AddRatingAsync(int movieId, int rating)
    {
        return _dbConnection.ExecuteAsync("INSERT INTO ratings (movie_id, rating) VALUES (@MovieId, @Rating)", new { MovieId = movieId, Rating = rating });
    }

    public async Task<IEnumerable<int>> GetRatingsAsync(int movieId)
    {
        return await _dbConnection.QueryAsync<int>("SELECT rating FROM ratings WHERE movie_id = @MovieId", new { MovieId = movieId });
    }
}
