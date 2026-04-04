namespace news.feed.Config.Settings.DbSettings;

public class MySqlTestSettingsStorage : IDbSettings
{
    private readonly string? _dbUser = "root";
    private readonly string? _dbPassword = "1234";
    private readonly string? _dbHost = "localhost";

    public string ConnectionString => $"server={_dbHost};database=news-feed;user={_dbUser};password={_dbPassword}";
}