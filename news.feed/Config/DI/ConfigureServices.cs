using Microsoft.EntityFrameworkCore;
using news.feed.Config.EntityFramework;
using news.feed.Config.Settings.DbSettings;
using news.feed.Repository;
using news.feed.Services;

namespace news.feed.Config.DI;

public static class ConfigureServices
{
    public static void ConfigureServiceCollection(this IServiceCollection services)
    {
        services.AddControllers();
        services.ConfigureDbSettings();
        services.ConfigureDependencies();
    }

    private static void ConfigureDependencies(this IServiceCollection services)
    {
        services.AddScoped<INewsService, NewsService>();
        services.AddScoped<INewsRepository, NewsRepository>();
        services.AddScoped<ProgramValidator>();
    }

    private static void ConfigureDbSettings(this IServiceCollection services)
    {
#if DEBUG
        services.AddSingleton<IDbSettings, MySqlTestSettingsStorage>();
#else
        services.AddSingleton<IDbSettings, MySqlSettingsStorage>();
#endif
        services.AddDbContext<NewsFeedContext>((serviceProvider, options) =>
        {
            var dbSettings = serviceProvider.GetService<IDbSettings>();
            ArgumentNullException.ThrowIfNull(dbSettings);
            options.UseMySql(dbSettings.ConnectionString, ServerVersion.AutoDetect(dbSettings.ConnectionString));
        });
    }
}