namespace news.feed.Config.Settings;

public class SettingsStorage
{
    private static readonly Lazy<SettingsStorage> instance = new(() => new SettingsStorage());
    private string? _dbUser;
    private string? _dbPassword;
    private string? _dbHost;

    public static SettingsStorage Instance => instance.Value;
    public string ConnectionString => $"server={_dbHost};database=news-feed;user={_dbUser};password={_dbPassword}";
    private SettingsStorage()
    {
        ReadSecrets();
    }

    private void ReadSecrets()
    {
        _dbUser = Environment.GetEnvironmentVariable("DB_USER");
        _dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
        _dbHost = Environment.GetEnvironmentVariable("DB_HOST");

        if (string.IsNullOrEmpty(_dbUser) || string.IsNullOrEmpty(_dbPassword))
            throw new NullReferenceException("Secrets were not found");
    }
}