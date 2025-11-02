using Microsoft.EntityFrameworkCore;
using news.feed.Config.EntityFramework;
using news.feed.Config.Settings;
using news.feed.News;

namespace news.feed.Config.DI;

public static class ConfigureServices
{
    public static void ConfigureServiceCollection(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddDbContext<NewsFeedContext>(options => 
            options.UseMySQL($"server=localhost;database={AppSettings.DataBase.Name};user=root;password={AppSettings.DataBase.RootPassword}"));
        services.ConfigureDependencies();
    }

    public static void ConfigureDependencies(this IServiceCollection services)
    {
        services.AddScoped<INewsService, NewsService>();
    }
}