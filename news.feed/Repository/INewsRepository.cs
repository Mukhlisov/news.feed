using news.feed.models;
using news.feed.models.Dto;
using news.feed.models.Models;

namespace news.feed.Repository;

public interface INewsRepository
{
    Task<News> CreateNewsAsync(NewsToSave newsToSave);
    Task<bool> UpdateNewsAsync(News news);
    Task<bool> UpdateNewsBodyAsync(NewsBody newsBody);
    Task<IEnumerable<News>> BatchGetNewsAsync(int skip = 0, int take = Consts.DefaultNewsBatchSize);
    Task<IEnumerable<News>> BatchGetNewsFromSpecifiedProgramAsync(string program, int skip = 0, int take = Consts.DefaultNewsBatchSize);
    Task<IReadOnlyList<(News News, NewsBody Body)>> GetEmbeddedNewsAsync(string program, int take = Consts.MaxEmbeddedNewsPerProgram);
    Task<int> CountEmbeddedNewsAsync(string program);
    Task<News> GetNewsByIdAsync(Guid id);
    Task<NewsBody> GetNewsBodyByIdAsync(Guid id);
    Task DeleteNewsAsync(Guid id);
    Task<bool> ChangeProgramAsync(News news);
}