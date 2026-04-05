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

    public async Task SaveNews(SaveNewsDto saveNewsDto)
    {
        var isValid = await _programValidator.CheckProgramIsValid(saveNewsDto.Program).ConfigureAwait(false);
        if (!isValid)
            throw new ValidationFailedException($"Program: {saveNewsDto.Program} doesn't exist");

        _ = await _newsRepository
            .SaveNews(NewsToSaveFactory.Create(saveNewsDto, AppSettings.DataBase.MainAuthorId))
            .ConfigureAwait(false);
    }

    public IEnumerable<News> GetBatchNewsFromSpecifiedProgram(string program, int skip = 0, int take = 0)
    {
        return _newsRepository.BatchGetNewsFromSpecifiedProgram(program,  skip, take);
    }
}