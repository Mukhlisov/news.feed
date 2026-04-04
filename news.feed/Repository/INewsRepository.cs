using news.feed.models.Dto;
using news.feed.models.Models;

namespace news.feed.Repository;

public interface INewsRepository
{
    public Task<Guid> SaveNews(NewsToSave newsToSave);
    public IEnumerable<News> BatchGetNews(int skip = 0, int take = 5);
    public IEnumerable<News> BatchGetNewsFromSpecifiedProgram(string program, int skip = 0, int take = 5);
}