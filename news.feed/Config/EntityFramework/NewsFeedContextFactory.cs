using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using news.feed.Config.Settings.DbSettings;

namespace news.feed.Config.EntityFramework;

public class NewsFeedContextFactory : IDesignTimeDbContextFactory<NewsFeedContext>
{
    public NewsFeedContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<NewsFeedContext>();

        var dbSettings = new MySqlTestSettingsStorage();
        optionsBuilder.UseMySql(dbSettings.ConnectionString, ServerVersion.AutoDetect(dbSettings.ConnectionString));

        return new NewsFeedContext(optionsBuilder.Options);
    }
}
