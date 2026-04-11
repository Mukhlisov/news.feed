using configuration.core;
using extra;
using news.feed.Config.DI;

namespace news.feed.Config;

public static class WebApplicationBuilderExtensions
{
    public static void ConfigureBuilder(this WebApplicationBuilder builder)
    {
        builder.ConfigureCors();
        builder.ConfigureKestrel();
        builder.InitAppSettings();
        builder.Logging.AddConsole();
        builder.Services.ConfigureServiceCollection();
    }

    private static void InitAppSettings(this WebApplicationBuilder builder) =>
        SettingsInitializer.InitSettings()
            .ForEach(x => builder.Services.AddSingleton(x.GetType(), x));
}