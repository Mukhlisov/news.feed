using configuration.core;
using news.feed.models;
using news.feed.models.Dto;
using news.feed.models.Exceptions;
using news.feed.models.Models;
using news.feed.Repository;
using news.feed.Utilities;
using UriBuilder = extra.UriBuilder;

namespace news.feed.Services;

public class NewsService : INewsService
{
    private readonly INewsRepository _newsRepository;
    private readonly AppSettings _appSettings;
    
    public NewsService(INewsRepository newsRepository, AppSettings appSettings)
    {
        _newsRepository = newsRepository;
        _appSettings = appSettings;
    }

    public async Task<CreationResult<News>> CreateNewsAsync(CreateNewsDto createNewsDto)
    {
        var news = await _newsRepository
            .CreateNewsAsync(NewsFactory.Create(createNewsDto, _appSettings.MainAuthorId))
            .ConfigureAwait(false);
        var uri = new UriBuilder(_appSettings.Domain)
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
        var uri = new UriBuilder(_appSettings.Domain)
            .AppendSegment(news.Program)
            .AppendSegment(news.Id)
            .BuildHttps();
        return new CreationResult<News>(uri, updatedNews);
    }

    public async Task<IEnumerable<News>> BatchGetNewsAsync(int skip, int take = Consts.DefaultNewsBatchSize)
    {
        return await _newsRepository.BatchGetNewsAsync(skip, take).ConfigureAwait(false);
    }

    public async Task<IEnumerable<News>> BatchGetNewsFromSpecifiedProgramAsync(
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