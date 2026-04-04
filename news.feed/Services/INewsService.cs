using news.feed.models.Dto;
using news.feed.models.Models;

namespace news.feed.Services;

public interface INewsService
{
    public Task SaveNews(SaveNewsDto saveNewsDto);
    public IEnumerable<News> GetBatchNewsFromSpecifiedProgram(string program, int skip = 0, int take = 0);
}