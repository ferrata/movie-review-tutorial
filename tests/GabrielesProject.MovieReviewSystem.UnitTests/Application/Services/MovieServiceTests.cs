using System.Runtime.CompilerServices;
using FluentAssertions;
using FluentValidation;
using GabrielesProject.MovieReviewSystem.Application.DTOs;
using GabrielesProject.MovieReviewSystem.Application.Interfaces;
using GabrielesProject.MovieReviewSystem.Application.Services;
using Moq;

using Model = GabrielesProject.MovieReviewSystem.Domain.Entities;

namespace GabrielesProject.MovieReviewSystem.ServiceTests.Application.Services;

public class MovieServiceTests
{
    private readonly Mock<ICommentsService> _commentsService;
    private readonly Mock<IMovieRatingRepository> _movieRatingRepository;
    private readonly Mock<IMovieRepository> _movieRepository;
    private readonly Mock<IRatingValidator> _ratingValidator;

    private readonly MovieService _movieService;

    public MovieServiceTests()
    {
        _movieRepository = new Mock<IMovieRepository>();
        _movieRatingRepository = new Mock<IMovieRatingRepository>();
        _commentsService = new Mock<ICommentsService>();
        _ratingValidator = new Mock<IRatingValidator>();

        _movieService = new MovieService(
            _movieRepository.Object,
            _movieRatingRepository.Object,
            _commentsService.Object,
            _ratingValidator.Object
        );
    }

    [Fact]
    public async Task WhenGetMovieAsync_WithValidId_ThenReturnMovie()
    {
        // Arrange
        var movieId = 1;
        var movieFromDb = new Model.Movie { Id = movieId, Title = "Movie 1", Summary = "Summary 1" };
        _movieRepository.Setup(x => x.GetMovieAsync(movieId)).ReturnsAsync(movieFromDb);
        
        // Act
        var result = await _movieService.GetMovieAsync(movieId);
        
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(movieId);
        result.Title.Should().Be("Movie 1");
        result.Summary.Should().Be("Summary 1");
    }

    [Fact]
    public async Task WhenGetMovieAsync_WithInvalidId_ThenReturnNull()
    {
        // Arrange
        var movieId = 1;
        _movieRepository.Setup(x => x.GetMovieAsync(movieId)).ReturnsAsync((Model.Movie)null);
        
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _movieService.GetMovieAsync(movieId));
    }

    [Fact]
    public async Task WhenAddMovieAsync_WithValidArgs_ThenReturnMovie()
    {
        // Arrange
        var movieArgs = new CreateMovieArgs { Title = "Movie 1", Summary = "Summary 1" };
        var movieId = 1;
        var movieFromDb = new Model.Movie { Id = movieId, Title = "Movie 1", Summary = "Summary 1" };
        _movieRepository.Setup(x => x.AddMovieAsync(It.IsAny<Model.Movie>())).ReturnsAsync(movieId);
        _movieRepository.Setup(x => x.GetMovieAsync(movieId)).ReturnsAsync(movieFromDb);
        
        // Act
        var result = await _movieService.AddMovieAsync(movieArgs);
        
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(movieId);
        result.Title.Should().Be("Movie 1");
        result.Summary.Should().Be("Summary 1");
    }

    [Fact]
    public async Task WhenAddMovieAsync_WithInvalidArgs_ThenReturnNull()
    {
        // Arrange
        var movieArgs = new CreateMovieArgs { Title = "Movie 1", Summary = "Summary 1" };
        _movieRepository.Setup(x => x.AddMovieAsync(It.IsAny<Model.Movie>())).ReturnsAsync(0);
        
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _movieService.AddMovieAsync(movieArgs));
    }

    [Fact]
    public async Task WhenGetMoviesAsync_ThenReturnMovies()
    {
        // Arrange
        var moviesFromDb = new List<Model.Movie>
        {
            new() { Id = 1, Title = "Movie 1", Summary = "Summary 1" },
            new() { Id = 2, Title = "Movie 2", Summary = "Summary 2" }
        };
        _movieRepository.Setup(x => x.GetMoviesAsync()).ReturnsAsync(moviesFromDb);
        
        // Act
        var result = await _movieService.GetMoviesAsync();
        
        // Assert
        var movies = result as Movie[] ?? result.ToArray();
        movies.Should().NotBeNullOrEmpty();
        movies.Should().HaveCount(2);
        movies.Should().Contain(m => m.Id == 1 && m.Title == "Movie 1" && m.Summary == "Summary 1");
        movies.Should().Contain(m => m.Id == 2 && m.Title == "Movie 2" && m.Summary == "Summary 2");
    }

    [Fact]
    public async Task WhenGetMoviesAsync_WithRating_ThenReturnMovies()
    {
        // Arrange
        var moviesFromDb = new List<Model.Movie>
        {
            new() { Id = 1, Title = "Movie 1", Summary = "Summary 1" },
            new() { Id = 2, Title = "Movie 2", Summary = "Summary 2" }
        };
        _movieRepository.Setup(x => x.GetMoviesAsync(4, 5)).ReturnsAsync(moviesFromDb);
        _movieRatingRepository
            .Setup(x => x.GetRatingsAsync(It.Is<int>(id => id == 1)))
            .ReturnsAsync(new List<int> {1, 2, 3});
        _movieRatingRepository
            .Setup(x => x.GetRatingsAsync(It.Is<int>(id => id == 2)))
            .ReturnsAsync(new List<int> {3, 4, 5});

        // Act
        var result = await _movieService.GetMoviesAsync(4, 5);
        
        // Assert
        var movies = result as Movie[] ?? result.ToArray();
        movies.Should().NotBeNullOrEmpty();
        movies.Should().HaveCount(1);
        movies.Should().Contain(m => m.Id == 2 && m.Title == "Movie 2" && m.Summary == "Summary 2");
    }
    
    [Fact]
    public async Task WhenGetMoviesAsync_WithInvalidArgs_ThenThrowException()
    {
        // Arrange
        _ratingValidator
            .Setup(x => x.ValidateAndThrow(It.IsAny<int>()))
            .Throws(new ValidationException("Invalid rating"));

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _movieService.GetMoviesAsync(-1, 7));
    }

    [Fact]
    public async Task WhenRateMovieAsync_WithInvalidRating_ThenThrowException()
    {
        // Arrange
        _ratingValidator
            .Setup(x => x.ValidateAndThrow(It.IsAny<int>()))
            .Throws(new ValidationException("Invalid rating"));
        
        var movieId = 1;
        var rating = 6;
        
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _movieService.RateMovieAsync(movieId, rating));
    }
}