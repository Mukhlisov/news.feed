using configuration.core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace news.feed.Config.EntityFramework;

public class NewsFeedContextFactory : IDesignTimeDbContextFactory<NewsFeedContext>
{
    public NewsFeedContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<NewsFeedContext>();

        var dbSettings = new MySqlSettingsStorage();
        optionsBuilder.UseMySql(dbSettings.ConnectionString, ServerVersion.AutoDetect(dbSettings.ConnectionString));

        return new NewsFeedContext(optionsBuilder.Options);
    }
}
