using news.feed.models;

namespace news.feed.News;

public interface INewsRepository
{
    public Task<Guid> SaveNews(NewsToSave newsToSave);
}