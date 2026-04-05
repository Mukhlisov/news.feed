using Microsoft.EntityFrameworkCore;
using news.feed.Config.DI;
using news.feed.Config.EntityFramework;
using news.feed.Config.Settings;

namespace news.feed;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureBuilder(builder);
        var app = builder.Build();
        app.UseHttpsRedirection();
        app.MapControllers();

        InitDbIfNotExists(app);
        
        app.Run();
    }

    private static void InitDbIfNotExists(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var db = scope.ServiceProvider.GetRequiredService<NewsFeedContext>();
        db.Database.Migrate();
        if (db.Programs.Any())
            return;
        db.Programs.AddRange(AppSettings.DataBase.DefaultPrograms);
        db.SaveChanges();
    }

    private static void ConfigureBuilder(WebApplicationBuilder builder)
    {
        builder.Logging.AddConsole();
        builder.Services.ConfigureServiceCollection();
    }
}