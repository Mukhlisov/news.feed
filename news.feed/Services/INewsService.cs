using news.feed.models.Dto;
using news.feed.models.Models;

namespace news.feed.Services;

public interface INewsService
{
    Task SaveNewsAsync(SaveNewsDto saveNewsDto);
    Task<IEnumerable<News>> GetBatchNewsFromSpecifiedProgramAsync(string program, int skip = 0, int take = 0);
    Task<NewsBody> GetNewsBodyAsync(Guid id);
    Task DeleteNewsAsync(Guid id);
}