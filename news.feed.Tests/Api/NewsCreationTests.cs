using System.Net;
using FluentAssertions;
using news.feed.models.Dto;
using news.feed.Tests.Api.Helpers;
using Xunit;

namespace news.feed.Tests.Api;

/// <summary>
/// Tests for news creation scenarios (scenario #2 from scenarios.txt).
/// </summary>
[Collection("NewsFeed API Collection")]
public class NewsCreationTests : IAsyncLifetime
{
    private readonly NewsFeedApiFactory _factory;
    private NewsApiClient _client = null!;

    public NewsCreationTests(NewsFeedApiFactory factory)
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
    public async Task CreateNews_WithFullData_CreatesNewsBodyAndAttachments()
    {
        var dto = new CreateNewsDto(
            Title: "Full news",
            PreviewUrl: "https://example.com/preview.jpg",
            Body: "Some very important body text",
            Program: "patronage",
            AttachmentUris: new List<AttachmentsDto>
            {
                new(null, "https://cdn.example.com/doc.pdf"),
                new(null, "https://cdn.example.com/photo.jpg")
            });

        var (status, news) = await _client.CreateNewsAsync(dto);

        status.Should().Be(HttpStatusCode.Created);
        news.Should().NotBeNull();

        var (bodyStatus, body) = await _client.GetNewsBodyAsync(news!.BodyId);
        bodyStatus.Should().Be(HttpStatusCode.OK);
        body.Should().NotBeNull();
        body!.Body.Should().Be("Some very important body text");
        body.Attachments.Should().HaveCount(2);
        body.Attachments.Select(a => a.AttachmentUrl).Should().Contain("https://cdn.example.com/doc.pdf");
    }

    [Fact]
    public async Task CreateNews_WithEmptyBody_StillCreatesNewsBodyRecord()
    {
        var dto = new CreateNewsDto(
            Title: "News with empty body",
            PreviewUrl: "",
            Body: "",
            Program: "baby-walk",
            AttachmentUris: new List<AttachmentsDto>());

        var (status, news) = await _client.CreateNewsAsync(dto);

        status.Should().Be(HttpStatusCode.Created);

        var (bodyStatus, body) = await _client.GetNewsBodyAsync(news!.BodyId);
        bodyStatus.Should().Be(HttpStatusCode.OK);
        body.Should().NotBeNull();
        body!.Body.Should().BeEmpty();
        body.Attachments.Should().BeEmpty();
    }

    [Fact]
    public async Task CreateNews_WithoutAttachments_CreatesNoAttachmentRecords()
    {
        var dto = new CreateNewsDto(
            Title: "No attachments news",
            PreviewUrl: "",
            Body: "Body without attachments",
            Program: "education",
            AttachmentUris: new List<AttachmentsDto>());

        var (status, news) = await _client.CreateNewsAsync(dto);
        status.Should().Be(HttpStatusCode.Created);

        var (bodyStatus, body) = await _client.GetNewsBodyAsync(news!.BodyId);
        bodyStatus.Should().Be(HttpStatusCode.OK);
        body!.Attachments.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task CreateNews_WithEmptyOrWhitespaceTitle_ReturnsBadRequest(string title)
    {
        var dto = new CreateNewsDto(
            Title: title,
            PreviewUrl: "",
            Body: "Some body",
            Program: "patronage",
            AttachmentUris: new List<AttachmentsDto>());

        var (status, _) = await _client.CreateNewsAsync(dto);

        status.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("non-existent-program")]
    public async Task CreateNews_WithInvalidProgram_ReturnsBadRequest(string program)
    {
        var dto = new CreateNewsDto(
            Title: "Valid title",
            PreviewUrl: "",
            Body: "Body",
            Program: program,
            AttachmentUris: new List<AttachmentsDto>());

        var (status, _) = await _client.CreateNewsAsync(dto);

        status.Should().Be(HttpStatusCode.BadRequest);
    }
}