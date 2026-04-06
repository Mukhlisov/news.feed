using news.feed.models.Dto;
using news.feed.models.Models;

namespace news.feed.Repository;

public interface INewsRepository
{
    Task<Guid> SaveNewsAsync(NewsToSave newsToSave);
    Task<IEnumerable<News>> BatchGetNewsAsync(int skip = 0, int take = 5);
    Task<IEnumerable<News>> BatchGetNewsFromSpecifiedProgramAsync(string program, int skip = 0, int take = 5);
    Task<NewsBody> GetNewsBodyAsync(Guid id);
    Task DeleteNewsAsync(Guid id);
}