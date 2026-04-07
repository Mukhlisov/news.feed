using Microsoft.EntityFrameworkCore;
using news.feed.Config.EntityFramework;
using news.feed.models;
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

    public async Task<News> CreateNewsAsync(NewsToSave newsToSave)
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
            return entity.Entity;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw new FailToModifyDataException("Failed to save news");
        }
    }

    public async Task<IEnumerable<News>> BatchGetNewsAsync(int skip = 0, int take = Consts.DefaultNewsBatchSize) =>
        await _newsFeedContext.News
            .Skip(skip)
            .Take(take)
            .ToListAsync().ConfigureAwait(false);

    public async Task<IEnumerable<News>> BatchGetNewsFromSpecifiedProgramAsync(
        string program,
        int skip = 0,
        int take = Consts.DefaultNewsBatchSize)
    {
        return await _newsFeedContext.News
            .Where(news => news.Program.Equals(program))
            .Skip(skip)
            .Take(take)
            .ToListAsync().ConfigureAwait(false);
    }

    public async Task<NewsBody> GetNewsBodyByIdAsync(Guid id)
    {
        var newsBody = await _newsFeedContext.NewsBodies.FirstOrDefaultAsync(body => body.Id == id).ConfigureAwait(false);
        return newsBody ?? throw new DataNotFoundException($"News body with id {id} not found");
    }

    public async Task DeleteNewsAsync(Guid id)
    {        
        await using var transaction = await _newsFeedContext.Database.BeginTransactionAsync();
        try
        {
            var news = await _newsFeedContext.News.FirstOrDefaultAsync(n => n.Id == id).ConfigureAwait(false);
            if (news == null)
                throw new DataNotFoundException($"News with id {id} not found");

            var newsBody = await _newsFeedContext.NewsBodies.FirstOrDefaultAsync(body => body.Id == news.BodyId)
                .ConfigureAwait(false);

            if (newsBody is not null)
                _newsFeedContext.NewsBodies.Remove(newsBody);
            _newsFeedContext.News.Remove(news);
            await _newsFeedContext.SaveChangesAsync().ConfigureAwait(false);
            await transaction.CommitAsync().ConfigureAwait(false);
        }
        catch (DataNotFoundException)
        {
            throw;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw new FailToModifyDataException($"Failed to delete news with id {id}");
        }
    }
}