namespace news.feed.Config;

public static class WebApplicationExtensions
{
    public static void ConfigureApplication(this WebApplication app)
    {
        app.UseExceptionHandler(); 
        app.UseForwardedHeaders();
        app.UseRouting();

        app.UseCors();
        app.UseRateLimiter();

        app.MapControllers();
    }
}