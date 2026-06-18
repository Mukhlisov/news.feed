using news.feed.Repository;

namespace news.feed.Services.Programs;

public class ProgramsService : IProgramsService
{
    private readonly IProgramsRepository _programsRepository;

    public ProgramsService(IProgramsRepository programsRepository)
    {
        _programsRepository = programsRepository;
    }

    public async Task<IEnumerable<news.feed.models.Models.Program>> GetAllProgramsAsync()
    {
        return await _programsRepository.GetAllProgramsAsync().ConfigureAwait(false);
    }
}
