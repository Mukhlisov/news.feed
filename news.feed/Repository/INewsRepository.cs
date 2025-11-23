using news.feed.models.Dto;

namespace news.feed.Repository;

public interface INewsRepository
{
    public Task<Guid> SaveNews(NewsToSave newsToSave);
    public IEnumerable<GetNewsDto> BatchGetNews(int skip = 0, int take = 5);
    public IEnumerable<GetNewsDto> BatchGetNewsFromSpecifiedProgram(string program, int skip = 0, int take = 5);
}