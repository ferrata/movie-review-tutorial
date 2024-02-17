using Microsoft.AspNetCore.Mvc.Testing;

namespace GabrielesProject.MovieReviewSystem.IntegrationTests.WebApi;

public class MoviesTests: IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public MoviesTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task WhenGetMovies_ThenReturn200()
    {
        // Arrange
        using var client = _factory.CreateClient();

        // Act
        using var response = await client.GetAsync("/movies");
        
        // Assert
        response.EnsureSuccessStatusCode();
    }
}