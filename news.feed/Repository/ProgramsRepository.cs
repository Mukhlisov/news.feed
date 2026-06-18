using Microsoft.EntityFrameworkCore;
using news.feed.Config.EntityFramework;

namespace news.feed.Repository;

public class ProgramsRepository : IProgramsRepository
{
    private readonly NewsFeedContext _newsFeedContext;

    public ProgramsRepository(NewsFeedContext newsFeedContext)
    {
        _newsFeedContext = newsFeedContext;
    }

    public async Task<IEnumerable<news.feed.models.Models.Program>> GetAllProgramsAsync()
    {
        return await _newsFeedContext.Programs
            .ToListAsync()
            .ConfigureAwait(false);
    }
}
