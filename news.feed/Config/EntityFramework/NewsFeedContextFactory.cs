using configuration.core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using news.feed.Config.Settings;

namespace news.feed.Config.EntityFramework;

public class NewsFeedContextFactory : IDesignTimeDbContextFactory<NewsFeedContext>
{
    public NewsFeedContext CreateDbContext(string[] args)
    {
        SettingsInitializer.InitSettings(ignoreEnvVarMiss: true);
        var optionsBuilder = new DbContextOptionsBuilder<NewsFeedContext>();
        optionsBuilder.UseNpgsql(PostgresSettingsStorage.ConnectionString);
        return new NewsFeedContext(optionsBuilder.Options);
    }
}
