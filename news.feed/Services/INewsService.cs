using news.feed.models;
using news.feed.models.Dto;
using news.feed.models.Models;

namespace news.feed.Services;

public interface INewsService
{
    Task<CreationResult<News>> CreateNewsAsync(CreateNewsDto createNewsDto);
    Task<CreationResult<News>> UpdateNewsAsync(UpdateNewsDto updateNewsDto);
    Task<IEnumerable<News>> BatchGetNewsAsync(int skip, int take = Consts.DefaultNewsBatchSize);
    Task<IEnumerable<News>> BatchGetNewsFromSpecifiedProgramAsync(string program, int skip = 0, int take = Consts.DefaultNewsBatchSize);
    Task<NewsBody> GetNewsBodyByIdAsync(Guid id);
    Task DeleteNewsAsync(Guid id);
}