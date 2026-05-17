using news.feed.Config.EntityFramework;
using news.feed.Config.settings;

namespace news.feed.Config;

public static class WebApplicationExtensions
{
    public static void ConfigureApplication(this WebApplication app)
    {
        app.MapControllers();
        app.UseForwardedHeaders();
        app.UseCors(AppSettings.Policies.AdminPanel);
        app.UseCors(AppSettings.Policies.DefaultSite);
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