namespace news.feed.Repository;

public interface IProgramsRepository
{
    Task<IEnumerable<news.feed.models.Models.Program>> GetAllProgramsAsync();
}
