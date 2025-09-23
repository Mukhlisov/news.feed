using news.feed.Config.Settings;

namespace news.feed.Config;

public partial class Configuration
{
    public const string ConnectionString = $"http://localhost:3306/{AppSettings.DataBase.Name}";
}