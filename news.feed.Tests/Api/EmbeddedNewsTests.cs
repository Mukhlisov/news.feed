using System.Net;
using FluentAssertions;
using news.feed.models;
using news.feed.models.Dto;
using news.feed.Tests.Api.Helpers;
using Xunit;

namespace news.feed.Tests.Api;

[Collection("NewsFeed API Collection")]
public class EmbeddedNewsTests : IAsyncLifetime
{
    private readonly NewsFeedApiFactory _factory;
    private NewsApiClient _client = null!;

    public EmbeddedNewsTests(NewsFeedApiFactory factory)
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

    private static CreateNewsDto EmbeddedNews(string title, string program = "patronage") =>
        new(title, "", $"{title} body", program,
            new List<AttachmentsDto> { new(null, $"https://img.com/{Guid.NewGuid()}.jpg") },
            IsEmbedded: true);

    private async Task CreateEmbeddedUpToLimitAsync(string program)
    {
        for (var i = 0; i < Consts.MaxEmbeddedNewsPerProgram; i++)
        {
            var (status, _) = await _client.CreateNewsAsync(EmbeddedNews($"Embedded {i}", program));
            status.Should().Be(HttpStatusCode.Created);
        }
    }

    [Fact]
    public async Task EmbeddedNews_IsExcludedFromProgramFeed_ButReturnedByEmbeddedEndpointWithBody()
    {
        var (_, regular) = await _client.CreateNewsAsync(new CreateNewsDto("Regular", "", "Regular body", "patronage"));
        var (_, embedded) = await _client.CreateNewsAsync(EmbeddedNews("Embedded"));

        var feed = await _client.GetProgramNewsAsync("patronage");
        feed.Select(n => n.Id).Should().BeEquivalentTo(new[] { regular!.Id });

        var (status, embeddedNews) = await _client.GetEmbeddedNewsAsync("patronage");
        status.Should().Be(HttpStatusCode.OK);
        embeddedNews.Should().ContainSingle();
        embeddedNews![0].Id.Should().Be(embedded!.Id);
        embeddedNews[0].IsEmbedded.Should().BeTrue();
        embeddedNews[0].Body.Should().Be("Embedded body");
        embeddedNews[0].AttachmentsUris.Should().ContainSingle();
    }

    [Fact]
    public async Task EmbeddedNews_IsStillAvailableInGeneralFeedAndById()
    {
        var (_, embedded) = await _client.CreateNewsAsync(EmbeddedNews("Embedded"));

        var feed = await _client.GetNewsAsync();
        feed.Select(n => n.Id).Should().Contain(embedded!.Id);

        var (status, byId) = await _client.GetNewsByIdAsync(embedded.Id);
        status.Should().Be(HttpStatusCode.OK);
        byId.IsEmbedded.Should().BeTrue();
    }

    [Fact]
    public async Task EmbeddedEndpoint_ReturnsNewestFirst()
    {
        await _client.CreateNewsAsync(EmbeddedNews("First"));
        await _client.CreateNewsAsync(EmbeddedNews("Second"));

        var (_, embeddedNews) = await _client.GetEmbeddedNewsAsync("patronage");

        embeddedNews!.Select(n => n.Title).Should().Equal("Second", "First");
    }

    [Fact]
    public async Task EmbeddedEndpoint_WithInvalidProgram_ReturnsBadRequest()
    {
        var (status, _) = await _client.GetEmbeddedNewsAsync("this-program-does-not-exist");

        status.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateNews_TogglesEmbeddedFlag_AndKeepsItWhenNotSpecified()
    {
        var (_, created) = await _client.CreateNewsAsync(new CreateNewsDto("Title", "", "Body", "patronage"));

        var status = await _client.UpdateNewsAsync(new UpdateNewsDto(created!.Id, "Title", "", "Body", IsEmbedded: true));
        status.Should().Be(HttpStatusCode.Created);
        (await _client.GetNewsByIdAsync(created.Id)).news.IsEmbedded.Should().BeTrue();

        status = await _client.UpdateNewsAsync(new UpdateNewsDto(created.Id, "New title", "", "Body"));
        status.Should().Be(HttpStatusCode.Created);
        (await _client.GetNewsByIdAsync(created.Id)).news.IsEmbedded.Should().BeTrue();

        status = await _client.UpdateNewsAsync(new UpdateNewsDto(created.Id, "New title", "", "Body", IsEmbedded: false));
        status.Should().Be(HttpStatusCode.Created);
        (await _client.GetNewsByIdAsync(created.Id)).news.IsEmbedded.Should().BeFalse();
        (await _client.GetProgramNewsAsync("patronage")).Select(n => n.Id).Should().Contain(created.Id);
    }

    [Fact]
    public async Task CreateEmbeddedNews_OverLimit_ReturnsBadRequest()
    {
        await CreateEmbeddedUpToLimitAsync("patronage");

        var (status, _) = await _client.CreateNewsAsync(EmbeddedNews("One too many"));
        status.Should().Be(HttpStatusCode.BadRequest);

        // Лимит действует на программу, а не глобально
        var (otherProgramStatus, _) = await _client.CreateNewsAsync(EmbeddedNews("Other program", "baby-walk"));
        otherProgramStatus.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task UpdateNewsToEmbedded_OverLimit_ReturnsBadRequest()
    {
        await CreateEmbeddedUpToLimitAsync("patronage");
        var (_, regular) = await _client.CreateNewsAsync(new CreateNewsDto("Regular", "", "Body", "patronage"));

        var status = await _client.UpdateNewsAsync(new UpdateNewsDto(regular!.Id, "Regular", "", "Body", IsEmbedded: true));

        status.Should().Be(HttpStatusCode.BadRequest);
        (await _client.GetNewsByIdAsync(regular.Id)).news.IsEmbedded.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateAlreadyEmbeddedNews_AtLimit_Succeeds()
    {
        await CreateEmbeddedUpToLimitAsync("patronage");
        var (_, embeddedNews) = await _client.GetEmbeddedNewsAsync("patronage");
        var target = embeddedNews![0];

        var status = await _client.UpdateNewsAsync(new UpdateNewsDto(target.Id, "Edited", "", "Body", IsEmbedded: true));

        status.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task ChangeProgramOfEmbeddedNews_ToProgramAtLimit_ReturnsBadRequest()
    {
        await CreateEmbeddedUpToLimitAsync("baby-walk");
        var (_, embedded) = await _client.CreateNewsAsync(EmbeddedNews("Moving"));

        var status = await _client.ChangeNewsProgramAsync(new ChangeNewsProgramDto(embedded!.Id, "baby-walk"));

        status.Should().Be(HttpStatusCode.BadRequest);
        (await _client.GetNewsByIdAsync(embedded.Id)).news.Program.Should().Be("patronage");
    }
}
