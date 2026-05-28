using System.Net;
using FluentAssertions;
using news.feed.models.Dto;
using news.feed.Tests.Api.Helpers;
using Xunit;

namespace news.feed.Tests.Api;

/// <summary>
/// Tests for news update scenarios (scenario #3).
/// </summary>
[Collection("NewsFeed API Collection")]
public class NewsUpdateTests : IAsyncLifetime
{
    private readonly NewsFeedApiFactory _factory;
    private NewsApiClient _client = null!;

    public NewsUpdateTests(NewsFeedApiFactory factory)
    {
        _factory = factory;
    }

    public async Task InitializeAsync()
    {
        await TestDatabaseHelper.ResetDatabaseAsync(_factory);
        var httpClient = await _factory.CreateAuthenticatedClientAsync();
        _client = new NewsApiClient(httpClient);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task UpdateNews_WithEmptyTitle_ReturnsBadRequest()
    {
        // First create a valid news
        var createDto = new CreateNewsDto("Original title", "", "Original body", "patronage", new List<AttachmentsDto>());
        var (_, created) = await _client.CreateNewsAsync(createDto);

        var updateDto = new UpdateNewsDto(created!.Id, "", "", "New body", new List<AttachmentsDto>());

        var status = await _client.UpdateNewsAsync(updateDto);

        status.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateNews_SuccessfullyUpdatesTitleBodyAndAddsAttachments()
    {
        var createDto = new CreateNewsDto(
            "Original title",
            "",
            "Original body",
            "patronage",
            new List<AttachmentsDto> { new(null, "https://old.com/1.pdf") });

        var (_, created) = await _client.CreateNewsAsync(createDto);

        var updateDto = new UpdateNewsDto(
            created!.Id,
            "Completely new title",
            "https://new-preview.jpg",
            "Completely new body text",
            new List<AttachmentsDto>
            {
                new(null, "https://new.com/extra.pdf") // new attachment
            });

        var status = await _client.UpdateNewsAsync(updateDto);
        status.Should().Be(HttpStatusCode.Created);

        var (_, body) = await _client.GetNewsBodyAsync(created.BodyId);

        body!.Body.Should().Be("Completely new body text");
        body.Attachments.Should().HaveCount(2); // 1 old + 1 new
        body.Attachments.Any(a => a.AttachmentUrl == "https://new.com/extra.pdf").Should().BeTrue();
    }
}