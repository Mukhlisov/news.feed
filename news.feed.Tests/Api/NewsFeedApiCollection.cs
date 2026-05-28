using Xunit;

namespace news.feed.Tests.Api;

/// <summary>
/// xUnit collection definition for tests that share a single Postgres container + application instance.
/// 
/// Using a collection fixture is significantly more efficient than IClassFixture when you have many tests,
/// because the expensive container startup happens only once.
/// 
/// Each individual test class can still opt into data reset via TestDatabaseHelper + IAsyncLifetime.
/// </summary>
[CollectionDefinition("NewsFeed API Collection")]
public class NewsFeedApiCollection : ICollectionFixture<NewsFeedApiFactory>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}