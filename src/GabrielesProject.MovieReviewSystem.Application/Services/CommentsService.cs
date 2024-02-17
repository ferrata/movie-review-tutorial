using System.Net.Http.Json;

namespace GabrielesProject.MovieReviewSystem.Application.Services;

public record ExternalComment
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Body { get; init; }
}

public interface ICommentsService
{
    Task<IEnumerable<ExternalComment>> GetCommentsAsync(int movieId);
}

public class CommentsService : ICommentsService
{
    private readonly HttpClient _httpClient;

    public CommentsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<ExternalComment>> GetCommentsAsync(int movieId)
    {
        var response = _httpClient.GetAsync($"https://jsonplaceholder.typicode.com/comments?postId={movieId}").Result;
        response.EnsureSuccessStatusCode();

        if (response.Content is null)
        {
            return [];
        }

        var result = await response.Content.ReadFromJsonAsync<IEnumerable<ExternalComment>>();
        return result ?? [];
    }
}
