using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using news.feed.Config.DI;
using news.feed.models.Policies;

namespace news.feed.Config;

public static class WebApplicationBuilderExtensions
{
    public static void ConfigureBuilder(this WebApplicationBuilder builder)
    {
        builder.ConfigureCors();
        builder.ConfigureKestrel();
        builder.Logging.AddConsole();
        builder.Services.ConfigureServiceCollection();

        builder.Services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter(policyName: nameof(Policies.LoginFixedWindowPolicy), fixedWindowOptions =>
            {
                fixedWindowOptions.PermitLimit = 10;
                fixedWindowOptions.Window = TimeSpan.FromMinutes(1);
                fixedWindowOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                fixedWindowOptions.QueueLimit = 1;
            });
        });
    }
}