namespace news.feed.Config.Settings;

public class TestSettingsStorage
{
    private static readonly Lazy<TestSettingsStorage> instance = new(() => new TestSettingsStorage());
    private string? _dbUser;
    private string? _dbPassword;
    private string? _dbHost;

    public static TestSettingsStorage Instance => instance.Value;
    public string ConnectionString => $"server={_dbHost};database=news-feed;user={_dbUser};password={_dbPassword}";

    public TestSettingsStorage()
    {
        _dbHost = "localhost";
        _dbUser = "root";
        _dbPassword = "1234";
    }
}