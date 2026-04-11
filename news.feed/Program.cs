using news.feed.Config;

namespace news.feed;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            var builder = WebApplication.CreateBuilder(args);
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