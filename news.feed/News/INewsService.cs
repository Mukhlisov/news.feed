using news.feed.models;

namespace news.feed.News;

public interface INewsService
{
    public Task SaveNews(MakeNewsDto makeNewsDto);
}