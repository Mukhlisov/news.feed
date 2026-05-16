using configuration.core;

namespace news.feed.Config.settings;

[Setting]
#pragma warning disable CS8618
public class PostgresSettingsStorage
{
    [Secret(Name = "CONNECTION_STRING")]
    public static string ConnectionString { get; set; }
}