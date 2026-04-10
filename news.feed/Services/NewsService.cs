using news.feed.Config.Settings;
using news.feed.models;
using news.feed.models.Dto;
using news.feed.models.Exceptions;
using news.feed.models.Models;
using news.feed.Repository;
using news.feed.Utilities;
using UriBuilder = news.feed.Utilities.UriBuilder;

namespace news.feed.Services;

public class NewsService : INewsService
{
    private readonly INewsRepository _newsRepository;

    public NewsService(INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    public async Task<CreationResult<News>> CreateNewsAsync(CreateNewsDto createNewsDto)
    {
        var news = await _newsRepository
            .CreateNewsAsync(NewsFactory.Create(createNewsDto, AppSettings.MainAuthorId))
            .ConfigureAwait(false);
        var uri = new UriBuilder(AppSettings.Domain)
            .AppendSegment(createNewsDto.Program)
            .AppendSegment(news.Id)
            .BuildHttps();

        return new CreationResult<News>(uri, news);
    }

    public async Task<CreationResult<News>> UpdateNewsAsync(UpdateNewsDto updateNewsDto)
    {
        var news = await _newsRepository.GetNewsByIdAsync(updateNewsDto.Id).ConfigureAwait(false);
        var isNewsBodyUpdated = await _newsRepository.UpdateNewsBodyAsync(new NewsBody
        {
            Id = news.BodyId,
            Body = updateNewsDto.Body
        }).ConfigureAwait(false);
        var updatedNews = NewsFactory.Create(news, updateNewsDto);
        var isNewsUpdated = await _newsRepository.UpdateNewsAsync(updatedNews)
            .ConfigureAwait(false);

        if (!isNewsBodyUpdated || !isNewsUpdated)
            throw new FailToModifyDataException($"Failed to update news: newsId = {news.Id}, bodyId = {news.BodyId}");
        var uri = new UriBuilder(AppSettings.Domain)
            .AppendSegment(news.Program)
            .AppendSegment(news.Id)
            .BuildHttps();
        return new CreationResult<News>(uri, updatedNews);
    }

    public async Task<IEnumerable<News>> GetNewsAsync(int skip, int take = Consts.DefaultNewsBatchSize)
    {
        return await _newsRepository.BatchGetNewsAsync(skip, take).ConfigureAwait(false);
    }

    public async Task<IEnumerable<News>> GetBatchNewsFromSpecifiedProgramAsync(
        string program,
        int skip = 0,
        int take = Consts.DefaultNewsBatchSize)
    {
        return await _newsRepository.BatchGetNewsFromSpecifiedProgramAsync(program, skip, take).ConfigureAwait(false);
    }

    public async Task<NewsBody> GetNewsBodyByIdAsync(Guid id) =>
        await _newsRepository.GetNewsBodyByIdAsync(id).ConfigureAwait(false);

    public async Task DeleteNewsAsync(Guid id)
    {
        await _newsRepository.DeleteNewsAsync(id).ConfigureAwait(false);
    }
}