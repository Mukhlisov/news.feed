using Microsoft.EntityFrameworkCore;
using news.feed.Config.EntityFramework;
using news.feed.Config.settings;
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

    private static void ConfigureDbSettings(this IServiceCollection services) =>
        services.AddDbContext<NewsFeedContext>(options => options.UseNpgsql(PostgresSettingsStorage.ConnectionString));
}