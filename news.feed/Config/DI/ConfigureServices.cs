using Microsoft.EntityFrameworkCore;
using news.feed.Config.EntityFramework;
using news.feed.Config.Settings;
using news.feed.Repository;
using news.feed.Services;

namespace news.feed.Config.DI;

public static class ConfigureServices
{
    public static void ConfigureServiceCollection(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddDbContext<NewsFeedContext>(options => 
            // options.UseMySql(SettingsStorage.Instance.ConnectionString, ServerVersion.AutoDetect(SettingsStorage.Instance.ConnectionString)));
            options.UseMySql(TestSettingsStorage.Instance.ConnectionString, ServerVersion.AutoDetect(TestSettingsStorage.Instance.ConnectionString)));
        services.ConfigureDependencies();
    }

    private static void ConfigureDependencies(this IServiceCollection services)
    {
        services.AddScoped<INewsService, NewsService>();
        services.AddScoped<INewsRepository, NewsRepository>();
    }
}