using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using news.feed.models.Dto;
using news.feed.Tests.Api.Helpers;
using Xunit;

namespace news.feed.Tests.Api;

/// <summary>
/// Tests for authentication and authorization behavior.
/// Covers scenario #1 from scenarios.txt.
/// </summary>
[Collection("NewsFeed API Collection")]
public class AuthorizationTests : IAsyncLifetime
{
    private readonly NewsFeedApiFactory _factory;
    private HttpClient _anonymousClient = null!;
    private NewsApiClient _authenticatedClient = null!;

    public AuthorizationTests(NewsFeedApiFactory factory)
    {
        _factory = factory;
    }

    public async Task InitializeAsync()
    {
        await TestDatabaseHelper.ResetDatabaseAsync(_factory);

        _anonymousClient = _factory.CreateClient();
        var authenticatedHttpClient = await _factory.CreateAuthenticatedClientAsync();
        _authenticatedClient = new NewsApiClient(authenticatedHttpClient);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    // ==================== Unauthorized access ====================

    [Fact]
    public async Task ProtectedEndpoints_Return401_WhenNoToken()
    {
        // Create
        var createResponse = await _anonymousClient.PostAsJsonAsync("/api/v1/news", new CreateNewsDto(
            "Some title", "", "body", "patronage", new List<AttachmentsDto>()));
        createResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        // Update - send a valid-shaped body so model validation passes and we reach the [Auth] filter
        var fakeUpdate = new UpdateNewsDto(Guid.NewGuid(), "Some title", "", "Some body", new List<AttachmentsDto>());
        var updateResponse = await _anonymousClient.PatchAsJsonAsync("/api/v1/news", fakeUpdate);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        // Delete news
        var deleteNewsResponse = await _anonymousClient.DeleteAsync($"/api/v1/news/{Guid.NewGuid()}");
        deleteNewsResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        // Delete attachment
        var deleteAttachmentResponse = await _anonymousClient.DeleteAsync($"/api/v1/attachments/{Guid.NewGuid()}");
        deleteAttachmentResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task PublicEndpoints_AreAccessible_WithoutToken()
    {
        var getNews = await _anonymousClient.GetAsync("/api/v1/news");
        getNews.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.BadRequest);

        var getProgram = await _anonymousClient.GetAsync("/api/v1/news/patronage");
        getProgram.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.BadRequest);

        var getBody = await _anonymousClient.GetAsync($"/api/v1/news/body/{Guid.NewGuid()}");
        getBody.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ==================== Authorized happy path ====================

    [Fact]
    public async Task FullCrudFlow_WithValidToken_Succeeds()
    {
        // Arrange
        var createDto = new CreateNewsDto(
            Title: "Test news title",
            PreviewUrl: "https://example.com/preview.jpg",
            Body: "This is the body of the news",
            Program: "patronage",
            AttachmentUris: new List<AttachmentsDto>
            {
                new(null, "https://example.com/file1.pdf"),
                new(null, "https://example.com/image.png")
            });

        // Act + Assert - Create
        var (createStatus, createdNews) = await _authenticatedClient.CreateNewsAsync(createDto);
        createStatus.Should().Be(HttpStatusCode.Created);
        createdNews.Should().NotBeNull();
        createdNews!.Title.Should().Be("Test news title");

        var (bodyStatus, body) = await _authenticatedClient.GetNewsBodyAsync(createdNews.BodyId);
        bodyStatus.Should().Be(HttpStatusCode.OK);
        body.Should().NotBeNull();
        body!.Attachments.Should().HaveCount(2);

        // Act + Assert - Update
        var updateDto = new UpdateNewsDto(
            Id: createdNews.Id,
            Title: "Updated title",
            PreviewUrl: createdNews.PreviewUrl,
            Body: "Updated body content",
            Attachments: new List<AttachmentsDto>
            {
                new(null, "https://example.com/new-attachment.pdf") // add one more
            });

        var updateStatus = await _authenticatedClient.UpdateNewsAsync(updateDto);
        updateStatus.Should().Be(HttpStatusCode.Created); // controller returns Created on update too

        var (updatedBodyStatus, updatedBody) = await _authenticatedClient.GetNewsBodyAsync(createdNews.BodyId);
        updatedBodyStatus.Should().Be(HttpStatusCode.OK);
        updatedBody!.Body.Should().Be("Updated body content");
        updatedBody.Attachments.Should().HaveCount(3); // 2 old + 1 new

        // Act + Assert - Delete
        var deleteStatus = await _authenticatedClient.DeleteNewsAsync(createdNews.Id);
        deleteStatus.Should().Be(HttpStatusCode.NoContent);

        var (finalBodyStatus, _) = await _authenticatedClient.GetNewsBodyAsync(createdNews.BodyId);
        finalBodyStatus.Should().Be(HttpStatusCode.NotFound);
    }
}