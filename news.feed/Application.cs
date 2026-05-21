using configuration.core;
using news.feed.Config;

namespace news.feed;

public static class Application
{
    public static void Run()
    {
        try
        {
            SettingsInitializer.InitSettings();
            var builder = WebApplication.CreateBuilder();
            builder.ConfigureBuilder();
            var app = builder.Build();
            app.FillProgramsTableIfNotExists();
            app.ConfigureApplication();
            app.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while starting the application: {ex.Message}");
        }
    }
}