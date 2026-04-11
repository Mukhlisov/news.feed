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

    /// <summary>
    /// Анти-паттерн, но иногда может быть полезен для получения сервисов на этапе конфигурации приложения,
    /// например, для получения настроек (как это получилось с CORS) из DI контейнера.
    /// </summary>
    public static T GetRequiredService<T>(this WebApplicationBuilder builder) where T : notnull
    {
        using var scope = builder.Build().Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<T>();
    }

    private static void InitAppSettings(this WebApplicationBuilder builder)
    {
        SettingsInitializer.InitSettings().ForEach(x => builder.Services.AddSingleton(x.GetType(), x));
    }
}