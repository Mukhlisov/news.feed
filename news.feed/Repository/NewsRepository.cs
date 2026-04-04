using news.feed.Config.EntityFramework;
using news.feed.models.Dto;
using news.feed.models.Exceptions;
using news.feed.models.Models;

namespace news.feed.Repository;

public class NewsRepository : INewsRepository
{
    private readonly NewsFeedContext _newsFeedContext;

    public NewsRepository(NewsFeedContext newsFeedContext)
    {
        _newsFeedContext = newsFeedContext;
    }

    public async Task<Guid> SaveNews(NewsToSave newsToSave)
    {
        await using var transaction = await _newsFeedContext.Database.BeginTransactionAsync();
        try
        {
            var body = await _newsFeedContext.NewsBodies.AddAsync(new NewsBody { Body = newsToSave.Body })
                .ConfigureAwait(false);
            var entity = await _newsFeedContext.News.AddAsync(new News
            {
                Program = newsToSave.Program,
                Title = newsToSave.Title,
                BodyId = body.Entity.Id,
                CreationTime = newsToSave.CreationDate,
                UpdateTime = newsToSave.LastUpdateDate,
                AuthorId = newsToSave.CreatorId
            }).ConfigureAwait(false);
            await _newsFeedContext.SaveChangesAsync().ConfigureAwait(false);
            await transaction.CommitAsync().ConfigureAwait(false);
            return entity.Entity.Id;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw new FailToModifyDataException("Failed to save news");
        }
    }

    public IEnumerable<GetNewsDto> BatchGetNews(int skip = 0, int take = 5) =>
        _newsFeedContext.News
            .Skip(skip)
            .Take(take)
            .Select(news =>
                new GetNewsDto(news.Id, news.Title, news.Program, news.BodyId, news.CreationTime, news.UpdateTime));

    public IEnumerable<GetNewsDto> BatchGetNewsFromSpecifiedProgram(string program, int skip = 0, int take = 5) =>
        _newsFeedContext.News
            .Skip(skip)
            .Take(take)
            .Where(news => news.Program.Equals(program))
            .Select(news =>
                new GetNewsDto(news.Id, news.Title, news.Program, news.BodyId, news.CreationTime, news.UpdateTime));
}