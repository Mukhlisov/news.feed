using Microsoft.EntityFrameworkCore;
using news.feed.Config.EntityFramework;

namespace news.feed.Services;

public class ProgramValidator(NewsFeedContext newsFeedContext)
{
    private readonly NewsFeedContext _newsFeedContext = newsFeedContext;

    public Task<bool> CheckProgramIsValid(string program) => _newsFeedContext.Programs.AnyAsync(p => p.Name == program);
}