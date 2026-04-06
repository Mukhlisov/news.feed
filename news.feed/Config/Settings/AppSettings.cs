namespace news.feed.Config.Settings;

using Program = models.Models.Program;

public static class AppSettings
{
    public const int DefaultNewsBatchSize = 10;
    public static readonly Guid MainAuthorId = new("67c368a9-97ed-4ef2-ba3f-e7eb9d2946e7");

    public static class DataBase
    {
        public const int ProgramAliasLength = 25;

        public static readonly Program[] DefaultPrograms = new[]
        {
            new Program { Alias = "patronage", Name = "Проект «Патронаж»" },
            new Program { Alias = "baby-walk", Name = "Движение BabyWalk Удмуртия" },
            new Program { Alias = "education", Name = "Просветительская деятельность в области реабилитации" },
        };
    }
}