using extra;
using news.feed.models.Models;

namespace configuration.core;

[Setting]
#pragma warning disable CS8618 // Non-nullable property must contain a non-null value when exiting constructor.
public class AppSettings
{
    [Secret(Name = "SITE_DOMAIN")]
    public string Domain { get; set; }
    [Secret(Name = "ADMIN_PANEL_DOMAIN")]
    public string AdminPanelDomain { get; set; } = "admin.babywalk.ru";
    [Secret(Name = "AUTHOR_ID", Override = true)]
    public Guid MainAuthorId { get; set; } //= new("67c368a9-97ed-4ef2-ba3f-e7eb9d2946e7");

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

        public static readonly Program[] DefaultPrograms = new[]
        {
            new Program { Alias = "patronage", Name = "Проект «Патронаж»" },
            new Program { Alias = "baby-walk", Name = "Движение BabyWalk Удмуртия" },
            new Program { Alias = "education", Name = "Просветительская деятельность в области реабилитации" },
        };
    }
}