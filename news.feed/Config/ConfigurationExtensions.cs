using extra;
using configuration.core;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using UriBuilder = extra.UriBuilder;

namespace news.feed.Config;

public static class ConfigurationExtensions
{
    public static void ConfigureCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            var (adminPanel, site) = GetUrisForCorsPolicy();
            options.AddPolicy(AppSettings.Policies.AdminPanel, policyBuilder =>
            {
                policyBuilder
                    .WithOrigins(adminPanel)
                    .AllowAnyMethod()
                    .WithHeaders(
                        // "Authorization",
                        "Content-Type"
                    );
            });
            options.AddPolicy(AppSettings.Policies.DefaultSite, policyBuilder =>
            {
                policyBuilder
                    .WithOrigins(site, adminPanel)
                    .WithMethods("GET");
            });
        });
    }

    public static void ConfigureKestrel(this WebApplicationBuilder builder)
    {
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenLocalhost(5000);
            options.Limits.KeepAliveTimeout = AppSettings.Kestrel.KeepAliveTimeout;
            options.Limits.MaxRequestBodySize = AppSettings.Kestrel.MaxRequestBodySize;
            options.Limits.MinResponseDataRate = new MinDataRate(
                bytesPerSecond: 50.Kilobytes(),
                gracePeriod: 10.Seconds());
            options.Limits.MinRequestBodyDataRate = new MinDataRate(
                bytesPerSecond: 50.Kilobytes(),
                gracePeriod: 10.Seconds());
        });
    }

    private static (string, string) GetUrisForCorsPolicy()
    {
#if DEBUG
        var adminPanel = new UriBuilder("localhost:3001").BuildHttp().GetLeftPart(UriPartial.Authority);
        var site = new UriBuilder("localhost:3000").BuildHttp().GetLeftPart(UriPartial.Authority);
#else
        var adminPanel = new UriBuilder(AppSettings.AdminPanelDomain).BuildHttps().GetLeftPart(UriPartial.Authority);
        var site = new UriBuilder(AppSettings.Domain).BuildHttps().GetLeftPart(UriPartial.Authority);
#endif
        return (adminPanel, site);
    }
}