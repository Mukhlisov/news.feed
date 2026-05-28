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

    public async Task<HttpResponseMessage> CreateNewsRawAsync(CreateNewsDto dto)
    {
        return await _client.PostAsJsonAsync("/api/v1/news", dto);
    }

    public async Task<(HttpStatusCode Status, News? Result)> CreateNewsAsync(CreateNewsDto dto)
    {
        var response = await CreateNewsRawAsync(dto);
        if (response.IsSuccessStatusCode)
        {
            // The controller returns Created(..., result.Result) so the body is the News directly
            var news = await response.Content.ReadFromJsonAsync<News>();
            return (response.StatusCode, news);
        }
        return (response.StatusCode, null);
    }

    public async Task<HttpResponseMessage> UpdateNewsRawAsync(UpdateNewsDto dto)
    {
        return await _client.PatchAsJsonAsync("/api/v1/news", dto);
    }

    public async Task<HttpStatusCode> UpdateNewsAsync(UpdateNewsDto dto)
    {
        var response = await UpdateNewsRawAsync(dto);
        return response.StatusCode;
    }

    public async Task<HttpResponseMessage> DeleteNewsRawAsync(Guid id)
    {
        return await _client.DeleteAsync($"/api/v1/news/{id}");
    }

    public async Task<HttpStatusCode> DeleteNewsAsync(Guid id)
    {
        var response = await DeleteNewsRawAsync(id);
        return response.StatusCode;
    }

    public async Task<(HttpStatusCode Status, NewsBody? Body)> GetNewsBodyAsync(Guid bodyId)
    {
        var response = await _client.GetAsync($"/api/v1/news/body/{bodyId}");
        if (response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadFromJsonAsync<NewsBody>();
            return (response.StatusCode, body);
        }
        return (response.StatusCode, null);
    }

    // ==================== Attachments ====================

    public async Task<HttpStatusCode> DeleteAttachmentAsync(Guid attachmentId)
    {
        var response = await _client.DeleteAsync($"/api/v1/attachments/{attachmentId}");
        return response.StatusCode;
    }
}