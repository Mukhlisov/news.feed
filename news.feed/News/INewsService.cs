using news.feed.models.Dto;

namespace news.feed.News;

public interface INewsService
{
    public Task SaveNews(MakeNewsDto makeNewsDto);
}