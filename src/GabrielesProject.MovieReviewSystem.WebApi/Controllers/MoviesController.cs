using GabrielesProject.MovieReviewSystem.Application.DTOs;
using GabrielesProject.MovieReviewSystem.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GabrielesProject.MovieReviewSystem.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class MoviesController : ControllerBase
{
    private readonly ILogger<MoviesController> _logger;
    private readonly IMovieService _service;

    public MoviesController(ILogger<MoviesController> logger, IMovieService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet]
    public async Task<IEnumerable<Movie>> Get([FromQuery] int? minRating, [FromQuery] int? maxRating)
    {
        if (minRating is null && maxRating is null)
        {
            return await _service.GetMoviesAsync();
        }

        return await _service.GetMoviesAsync(minRating, maxRating);
    }

    [HttpGet("{id}")]
    public async Task<Movie> Get(int id)
    {
        return await _service.GetMovieAsync(id);
    }

    [HttpPost]
    public async Task<Movie> Post([FromBody] CreateMovieArgs movie)
    {
        return await _service.AddMovieAsync(movie);
    }

    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
        await _service.DeleteMovieAsync(id);
    }

    [HttpPost("{id}/ratings")]
    public async Task Rate(int id, [FromBody] int rating)
    {
        await _service.RateMovieAsync(id, rating);
    }
}
