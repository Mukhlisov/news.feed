using news.feed.Config;
using news.feed.Config.DI;
using news.feed.Config.EntityFramework;
using news.feed.Config.Settings;

namespace news.feed;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureBuilder(builder);
            var app = builder.Build();
            app.MapControllers();

            FillProgramsTableIfNotExists(app);

            app.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while starting the application: {ex.Message}");
        }
    }

    private static void FillProgramsTableIfNotExists(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var db = scope.ServiceProvider.GetRequiredService<NewsFeedContext>();
        if (db.Programs.Any())
            return;
        db.Programs.AddRange(AppSettings.DataBase.DefaultPrograms);
        db.SaveChanges();
    }

    private static void ConfigureBuilder(WebApplicationBuilder builder)
    {
        builder.ConfigureCors();
        builder.ConfigureKestrel();
        builder.Logging.AddConsole();
        builder.Services.ConfigureServiceCollection();
    }
}