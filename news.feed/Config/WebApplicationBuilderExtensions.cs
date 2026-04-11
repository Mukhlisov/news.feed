using news.feed.Config.DI;

namespace news.feed.Config;

public static class WebApplicationBuilderExtensions
{
    public static void ConfigureBuilder(this WebApplicationBuilder builder)
    {
        builder.ConfigureCors();
        builder.ConfigureKestrel();
        builder.Logging.AddConsole();
        builder.Services.ConfigureServiceCollection();
    }
}