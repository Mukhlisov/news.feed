using Microsoft.AspNetCore.Server.Kestrel.Core;
using news.feed.Config.Settings;
using UriBuilder = news.feed.Utilities.UriBuilder;

namespace news.feed.Config;

public static class AppConfigurationExtensions
{
    // Возможно смогу как-то права тут настроить
    public static void ConfigureCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            var uri = new UriBuilder(AppSettings.Domain).BuildHttps().AbsoluteUri;
            options.AddPolicy("", policyBuilder =>
            {
                policyBuilder
                    .WithOrigins(uri)
                    .WithOrigins()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }

    public static void ConfigureKestrel(this WebApplicationBuilder builder)
    {
        // В целом разобраться как конфигурировать Kestrel
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(5000);
            options.Limits.MaxConcurrentConnections = 1000;
            options.Limits.MaxRequestBodySize = 10 * 1024; // Увеличить размер реквеста
            options.Limits.MinResponseDataRate = new MinDataRate(
                bytesPerSecond: 240, gracePeriod: TimeSpan.FromSeconds(2));
        });
    }
}