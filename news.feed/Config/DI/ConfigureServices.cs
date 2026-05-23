using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using news.feed.Auth;
using news.feed.Config.EntityFramework;
using news.feed.Config.Settings;
using news.feed.Repository;
using news.feed.Services.Auth;
using news.feed.Services.Hashing;
using news.feed.Services.News;

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
        services.AddTransient<IMemoryCache, MemoryCache>();

        services.AddScoped<INewsService, NewsService>();
        services.AddScoped<INewsRepository, NewsRepository>();
        services.AddScoped<ProgramValidator>();

        services.AddScoped<IAttachmentsRepository, AttachmentsRepository>();

        services.AddSingleton<IHasher, Hasher>();
        services.AddSingleton<ISecretProvider, SecretProvider>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ISessionManager, SessionManager>();
        services.AddScoped<AuthAttribute>();
    }

    private static void ConfigureDbSettings(this IServiceCollection services) =>
        services.AddDbContext<NewsFeedContext>(options => options.UseNpgsql(PostgresSettingsStorage.ConnectionString));
}