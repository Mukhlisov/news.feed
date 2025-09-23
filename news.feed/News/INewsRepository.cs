using news.feed.models.Dto;

namespace news.feed.News;

public interface INewsRepository
{
    public Task<Guid> SaveNews(NewsToSave newsToSave);
}