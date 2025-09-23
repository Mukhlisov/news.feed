using Microsoft.EntityFrameworkCore;

namespace news.feed.Config.EntityFramework;

public class RegisterContext
{
    public IConfiguration Configuration { get; }

    public RegisterContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<NewsFeedContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))); // Gets setting from appsettings.json by parsing setting name. "DefaultConnection" for example

        services.AddControllersWithViews();
    }
}