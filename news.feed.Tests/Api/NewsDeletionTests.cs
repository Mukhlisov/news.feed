using System.Net;
using FluentAssertions;
using news.feed.models.Dto;
using news.feed.Tests.Api.Helpers;
using Xunit;

namespace news.feed.Tests.Api;

/// <summary>
/// Tests for full news deletion with cascade (scenario #4).
/// </summary>
[Collection("NewsFeed API Collection")]
public class NewsDeletionTests : IAsyncLifetime
{
    private readonly NewsFeedApiFactory _factory;
    private NewsApiClient _client = null!;

    public NewsDeletionTests(NewsFeedApiFactory factory)
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
    public async Task DeleteNews_RemovesNewsBodyAndAllAttachments()
    {
        // Arrange - create full news with attachments
        var createDto = new CreateNewsDto(
            "News to delete",
            "",
            "Body that will be deleted",
            "patronage",
            new List<AttachmentsDto>
            {
                new(null, "https://files.com/a.pdf"),
                new(null, "https://files.com/b.jpg"),
                new(null, "https://files.com/c.docx")
            });

        var (_, created) = await _client.CreateNewsAsync(createDto);

        // Verify it exists
        var (beforeDeleteStatus, beforeBody) = await _client.GetNewsBodyAsync(created!.BodyId);
        beforeDeleteStatus.Should().Be(HttpStatusCode.OK);
        beforeBody!.Attachments.Should().HaveCount(3);

        // Act
        var deleteStatus = await _client.DeleteNewsAsync(created.Id);
        deleteStatus.Should().Be(HttpStatusCode.NoContent);

        // Assert - everything is gone
        var (afterDeleteStatus, _) = await _client.GetNewsBodyAsync(created.BodyId);
        afterDeleteStatus.Should().Be(HttpStatusCode.NotFound);
    }
}