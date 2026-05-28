using System.Net;
using FluentAssertions;
using news.feed.models.Dto;
using news.feed.Tests.Api.Helpers;
using Xunit;

namespace news.feed.Tests.Api;

/// <summary>
/// Tests for deleting individual attachments (scenario #5).
/// </summary>
[Collection("NewsFeed API Collection")]
public class AttachmentDeletionTests : IAsyncLifetime
{
    private readonly NewsFeedApiFactory _factory;
    private NewsApiClient _client = null!;

    public AttachmentDeletionTests(NewsFeedApiFactory factory)
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
    public async Task DeleteAttachments_RemovesThemFromNewsBody()
    {
        // Create news with 3 attachments
        var createDto = new CreateNewsDto(
            "News with attachments to delete",
            "",
            "Body",
            "patronage",
            new List<AttachmentsDto>
            {
                new(null, "https://a.com/1.pdf"),
                new(null, "https://a.com/2.pdf"),
                new(null, "https://a.com/3.pdf")
            });

        var (createStatus, created) = await _client.CreateNewsAsync(createDto);
        createStatus.Should().Be(HttpStatusCode.Created);
        created.Should().NotBeNull();

        var (bodyBeforeStatus, bodyBefore) = await _client.GetNewsBodyAsync(created!.BodyId);
        bodyBeforeStatus.Should().Be(HttpStatusCode.OK);
        bodyBefore!.Attachments.Should().HaveCount(3);

        var attachmentToDelete = bodyBefore.Attachments.First();

        // Delete one attachment
        var deleteStatus = await _client.DeleteAttachmentAsync(attachmentToDelete.Id);
        deleteStatus.Should().Be(HttpStatusCode.NoContent);

        // Verify
        var (bodyAfterStatus, bodyAfter) = await _client.GetNewsBodyAsync(created.BodyId);
        bodyAfterStatus.Should().Be(HttpStatusCode.OK);
        bodyAfter!.Attachments.Should().HaveCount(2);
        bodyAfter.Attachments.Should().NotContain(a => a.Id == attachmentToDelete.Id);
    }
}