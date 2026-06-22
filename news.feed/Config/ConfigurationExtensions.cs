using extra;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using news.feed.Config.Settings;
using news.feed.models.Policies;
using UriBuilder = extra.UriBuilder;

namespace news.feed.Config;

public static class ConfigurationExtensions
{
    private static bool IsInContainer => Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

    public static void ConfigureCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            var (adminPanel, site) = GetUrisForCorsPolicy();
            options.AddPolicy(nameof(Policies.AdminPanelPolicy), policyBuilder =>
            {
                policyBuilder
                    .WithOrigins(adminPanel)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
            options.AddPolicy(nameof(Policies.GetNewsPolicy), policyBuilder =>
            {
                policyBuilder
                    .WithOrigins(site, adminPanel)
                    .WithMethods("GET")
                    .AllowAnyHeader();
            });
        });
    }

    public static void ConfigureKestrel(this WebApplicationBuilder builder)
    {

        var port = IsInContainer ? 8080 : 5000;
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(port);
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
        string adminPanel;
        string site;
        if (IsInContainer)
        {
            adminPanel = new UriBuilder(AppSettings.AdminPanelDomain).BuildHttps().GetLeftPart(UriPartial.Authority);
            site = new UriBuilder(AppSettings.Domain).BuildHttps().GetLeftPart(UriPartial.Authority);
        }
        else{
            adminPanel = new UriBuilder(AppSettings.AdminPanelDomain).BuildHttp().GetLeftPart(UriPartial.Authority);
            site = new UriBuilder(AppSettings.Domain).BuildHttp().GetLeftPart(UriPartial.Authority);
        }

        return (adminPanel, site);
    }
}