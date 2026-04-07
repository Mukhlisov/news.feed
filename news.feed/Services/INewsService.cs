using news.feed.models;
using news.feed.models.Dto;
using news.feed.models.Models;

namespace news.feed.Services;

public interface INewsService
{
    Task<CreationResult<News>> CreateNewsAsync(CreateNewsDto createNewsDto);
    Task<IEnumerable<News>> GetNewsAsync(int skip, int take = Consts.DefaultNewsBatchSize);
    Task<IEnumerable<News>> GetBatchNewsFromSpecifiedProgramAsync(string program, int skip = 0, int take = Consts.DefaultNewsBatchSize);
    Task<NewsBody> GetNewsBodyByIdAsync(Guid id);
    Task DeleteNewsAsync(Guid id);
}