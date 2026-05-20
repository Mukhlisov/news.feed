using news.feed.Config.EntityFramework;
using news.feed.Config.Settings;
using news.feed.models.Policies;

namespace news.feed.Config;

public static class WebApplicationExtensions
{
    public static void ConfigureApplication(this WebApplication app)
    {
        app.UseForwardedHeaders();
        app.UseRouting();

        app.UseCors(nameof(Policies.AdminPanelPolicy));
        app.UseCors(nameof(Policies.GetNewsPolicy));
        app.UseRateLimiter();

        app.MapControllers();
    }

    public static void FillProgramsTableIfNotExists(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var db = scope.ServiceProvider.GetRequiredService<NewsFeedContext>();
        if (db.Programs.Any())
            return;
        db.Programs.AddRange(AppSettings.DataBase.DefaultPrograms);
        db.SaveChanges();
    }
}