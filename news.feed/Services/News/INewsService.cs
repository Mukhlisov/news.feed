using news.feed.models;
using news.feed.models.Dto;
using news.feed.models.Models;

namespace news.feed.Services.News;

public interface INewsService
{
    Task<CreationResult<models.Models.News>> CreateNewsAsync(CreateNewsDto createNewsDto);
    Task<CreationResult<models.Models.News>> UpdateNewsAsync(UpdateNewsDto updateNewsDto);
    Task<IEnumerable<models.Models.News>> BatchGetNewsAsync(int skip, int take = Consts.DefaultNewsBatchSize);
    Task<IEnumerable<models.Models.News>> BatchGetNewsFromSpecifiedProgramAsync(string program, int skip = 0, int take = Consts.DefaultNewsBatchSize);
    Task<NewsBody> GetNewsBodyByIdAsync(Guid id);
    Task DeleteNewsAsync(Guid id);
}