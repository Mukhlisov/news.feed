using news.feed.Config.Settings;
using news.feed.models.Dto;
using news.feed.models.Exceptions;
using news.feed.models.Models;
using news.feed.Repository;
using Tools;

namespace news.feed.Services;

public class NewsService : INewsService
{
    private readonly INewsRepository _newsRepository;
    private readonly ProgramValidator _programValidator;

    public NewsService(INewsRepository newsRepository,
        ProgramValidator programValidator)
    {
        _newsRepository = newsRepository;
        _programValidator = programValidator;
    }

    public async Task SaveNewsAsync(SaveNewsDto saveNewsDto)
    {
        var isValid = await _programValidator.CheckProgramIsValid(saveNewsDto.Program).ConfigureAwait(false);
        if (!isValid)
            throw new ValidationFailedException($"Program: {saveNewsDto.Program} doesn't exist");

        _ = await _newsRepository
            .SaveNewsAsync(NewsToSaveFactory.Create(saveNewsDto, AppSettings.MainAuthorId))
            .ConfigureAwait(false);
    }

    public async Task<IEnumerable<News>> GetBatchNewsFromSpecifiedProgramAsync(
        string program,
        int skip = 0,
        int take = 0)
    {
        return await _newsRepository.BatchGetNewsFromSpecifiedProgramAsync(program, skip, take).ConfigureAwait(false);
    }

    public async Task<NewsBody> GetNewsBodyAsync(Guid id)
    {
        return await _newsRepository.GetNewsBodyAsync(id).ConfigureAwait(false);
    }

    public async Task DeleteNewsAsync(Guid id)
    {
        await _newsRepository.DeleteNewsAsync(id).ConfigureAwait(false);
    }
}