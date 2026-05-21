using configuration.core;

namespace news.feed.Config.Settings;

#pragma warning disable CS8618
[Setting]
public class AuthSettings
{
    [Secret(Name = "AUTH_ADMIN_NAME")]
    public static string AdminName { get; set; }

    [Secret(Name = "PASSWORD_HASH")]
    public static string PasswordHash { get; set; }
}