using System.Net;
using System.Net.Http.Json;
using news.feed.models.Dto;
using news.feed.models.Models;

namespace news.feed.Tests.Api.Helpers;

/// <summary>
/// High-level client for interacting with the News and Attachments APIs in tests.
/// Wraps an authenticated HttpClient.
/// </summary>
public class NewsApiClient
{
    private readonly HttpClient _client;

    public NewsApiClient(HttpClient client)
    {
        _client = client;
    }

    // ==================== News ====================

    public async Task<(HttpStatusCode Status, News? Result)> CreateNewsAsync(CreateNewsDto dto)
    {
        var response = await _client.PostAsJsonAsync("/api/v1/news", dto);
        if (!response.IsSuccessStatusCode) 
            return (response.StatusCode, null);
        // The controller returns Created(..., result.Result) so the body is the News directly
        var news = await response.Content.ReadFromJsonAsync<News>();
        return (response.StatusCode, news);
    }


    public async Task<HttpStatusCode> UpdateNewsAsync(UpdateNewsDto dto)
    {
        var response = await _client.PatchAsJsonAsync("/api/v1/news", dto);
        return response.StatusCode;
    }

    public async Task<HttpStatusCode> ChangeNewsProgramAsync(ChangeNewsProgramDto dto)
    {
        var response = await _client.PatchAsJsonAsync("/api/v1/news/change-program", dto);
        return response.StatusCode;
    }

    public async Task<HttpStatusCode> DeleteNewsAsync(Guid id)
    {
        var response = await _client.DeleteAsync($"/api/v1/news/{id}");
        return response.StatusCode;
    }

    public async Task<(HttpStatusCode Status, NewsBody? Body)> GetNewsBodyAsync(Guid bodyId)
    {
        var response = await _client.GetAsync($"/api/v1/news/body/{bodyId}");
        if (!response.IsSuccessStatusCode) 
            return (response.StatusCode, null);
        var body = await response.Content.ReadFromJsonAsync<NewsBody>();
        return (response.StatusCode, body);
    }

    public async Task<(HttpStatusCode Status, NewsDto news)> GetNewsByIdAsync(Guid id)
    {
        var response = await _client.GetAsync($"/api/v1/news/{id}");
        if (!response.IsSuccessStatusCode) 
            return (response.StatusCode, null!);
        var news = await response.Content.ReadFromJsonAsync<NewsDto>();
        return (response.StatusCode, news!);
    }

    // ==================== Attachments ====================

    public async Task<HttpStatusCode> DeleteAttachmentAsync(Guid attachmentId)
    {
        var response = await _client.DeleteAsync($"/api/v1/attachments/{attachmentId}");
        return response.StatusCode;
    }
}