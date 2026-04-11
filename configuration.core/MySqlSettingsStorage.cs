namespace configuration.core;

[Setting]
public class MySqlSettingsStorage
{
    [Secret(Name = "CONNECTION_STRING", Override = true)]
    public string ConnectionString { get; set; } = "server=localhost;database=news-feed;user=root;password=1234";
}