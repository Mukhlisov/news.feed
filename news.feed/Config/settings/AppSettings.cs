using configuration.core;
using extra;

namespace news.feed.Config.settings;

[Setting]
#pragma warning disable CS8618 // Non-nullable property must contain a non-null value when exiting constructor.
public class AppSettings
{
    [Secret(Name = "SITE_DOMAIN", Override = false)]
    public static string Domain { get; set; }
    [Secret(Name = "ADMIN_PANEL_DOMAIN", Override = false)]
    public static string AdminPanelDomain { get; set; } = "admin.babywalk.ru";
    [Secret(Name = "AUTHOR_ID")]
    public static Guid MainAuthorId { get; set; } //= new("67c368a9-97ed-4ef2-ba3f-e7eb9d2946e7");

    public static class Kestrel
    {
        public static readonly TimeSpan KeepAliveTimeout = 1.Minutes();
        public static readonly long MaxRequestBodySize = 5.Megabytes();
    }

    public static class Policies
    {
        public const string AdminPanel = "AdminPanelPolicy";
        public const string DefaultSite = "GetNewsPolicy";
    }

    public static class DataBase
    {
        public const int ProgramAliasLength = 25;

        public static readonly models.Models.Program[] DefaultPrograms = new[]
        {
            new models.Models.Program { Alias = "patronage", Name = "Проект «Патронаж»" },
            new models.Models.Program { Alias = "baby-walk", Name = "Движение BabyWalk Удмуртия" },
            new models.Models.Program { Alias = "education", Name = "Просветительская деятельность в области реабилитации" },
        };
    }
}