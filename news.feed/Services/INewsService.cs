using news.feed.models.Dto;

namespace news.feed.Services;

public interface INewsService
{
    public Task SaveNews(SaveNewsDto saveNewsDto);
    public string GetBatchNewsFromSpecifiedProgram(string program, int skip = 0, int take = 0);
}