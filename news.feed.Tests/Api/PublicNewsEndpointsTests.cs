using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using news.feed.models.Models;
using Xunit;

namespace news.feed.Tests.Api;

/// <summary>
/// Smoke / contract tests for the public (no authentication required) news endpoints.
/// These tests validate basic happy paths and error handling for unauthenticated clients.
/// </summary>
[Collection("NewsFeed API Collection")]
public class PublicNewsEndpointsTests : IAsyncLifetime
{
    private readonly NewsFeedApiFactory _factory;
    private readonly HttpClient _client;

    public PublicNewsEndpointsTests(NewsFeedApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        // Reset mutable data before each test to ensure isolation.
        // Static reference data (programs) is preserved by TestDatabaseHelper.
        await TestDatabaseHelper.ResetDatabaseAsync(_factory);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task GetNews_ReturnsOk_AndReturnsList()
    {
        // take must be <= Consts.DefaultNewsBatchSize (10) due to ValueRangeCheck attribute
        var response = await _client.GetAsync("/api/v1/news?skip=0&take=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var news = await response.Content.ReadFromJsonAsync<List<News>>();
        news.Should().NotBeNull();
    }

    [Fact]
    public async Task GetNewsFromValidProgram_ReturnsOk()
    {
        // "patronage" is one of the default seeded programs
        var response = await _client.GetAsync("/api/v1/news/patronage?skip=0&take=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var news = await response.Content.ReadFromJsonAsync<List<News>>();
        news.Should().NotBeNull();
    }

    [Fact]
    public async Task GetNewsFromInvalidProgram_ReturnsBadRequest()
    {
        var response = await _client.GetAsync("/api/v1/news/this-program-does-not-exist");

        // ProgramValidationAttribute + ProgramValidator should reject unknown programs
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetNewsBody_WithNonExistentId_Returns404()
    {
        var nonExistentId = Guid.NewGuid();

        var response = await _client.GetAsync($"/api/v1/news/body/{nonExistentId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
