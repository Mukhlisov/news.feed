using news.feed.Services.News;

namespace news.feed.Utilities;

public static class ProgramValidatorExtensions
{
    public static bool CheckProgramIsValidSync(this ProgramValidator validator, string program) =>
        validator.CheckProgramIsValidAsync(program)
            .GetAwaiter()
            .GetResult();
}