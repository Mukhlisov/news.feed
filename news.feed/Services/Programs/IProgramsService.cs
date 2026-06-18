namespace news.feed.Services.Programs;

public interface IProgramsService
{
    Task<IEnumerable<news.feed.models.Models.Program>> GetAllProgramsAsync();
}
