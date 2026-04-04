namespace news.feed.Config.Settings.DbSettings;

public class MySqlSettingsStorage : IDbSettings
{
    private string? _dbUser;
    private string? _dbPassword;
    private string? _dbHost;

    public string ConnectionString => $"server={_dbHost};database=news-feed;user={_dbUser};password={_dbPassword}";
    private MySqlSettingsStorage()
    {
        ReadSecrets();
    }

    private void ReadSecrets()
    {
        _dbUser = Environment.GetEnvironmentVariable("DB_USER");
        _dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
        _dbHost = Environment.GetEnvironmentVariable("DB_HOST");

        ArgumentNullException.ThrowIfNull(_dbUser);
        ArgumentNullException.ThrowIfNull(_dbPassword);
        ArgumentNullException.ThrowIfNull(_dbHost);
    }
}