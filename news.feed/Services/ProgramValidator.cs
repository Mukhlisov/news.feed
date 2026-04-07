using Microsoft.EntityFrameworkCore;
using news.feed.Config.EntityFramework;

namespace news.feed.Services;

public class ProgramValidator
{
    private readonly NewsFeedContext _newsFeedContext;

    public ProgramValidator(NewsFeedContext newsFeedContext)
    {
        _newsFeedContext = newsFeedContext;
    }

    public Task<bool> CheckProgramIsValidAsync(string program) => 
        _newsFeedContext.Programs.AnyAsync(p => p.Alias == program);
}