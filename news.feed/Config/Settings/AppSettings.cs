using configuration.core;
using extra;

namespace news.feed.Config.Settings;

[Setting]
#pragma warning disable CS8618 // Non-nullable property must contain a non-null value when exiting constructor.
public class AppSettings
{
    [Secret(Name = "SITE_DOMAIN")]
    public static string Domain { get; set; }
    [Secret(Name = "ADMIN_PANEL_DOMAIN")]
    public static string AdminPanelDomain { get; set; } = "admin.babywalk.ru";
    [Secret(Name = "AUTHOR_ID")]
    public static Guid MainAuthorId { get; set; } //= new("67c368a9-97ed-4ef2-ba3f-e7eb9d2946e7");

    public static class Kestrel
    {
        public static readonly TimeSpan KeepAliveTimeout = 1.Minutes();
        public static readonly long MaxRequestBodySize = 5.Megabytes();
    }
}