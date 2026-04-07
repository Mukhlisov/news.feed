using news.feed.Services;

namespace news.feed.Utilities;

public static class ProgramValidatorExtensions
{
    public static bool CheckProgramIsValidSync(this ProgramValidator validator, string program) =>
        validator.CheckProgramIsValidAsync(program)
            .GetAwaiter()
            .GetResult();
}