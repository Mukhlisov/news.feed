using Respawn;
using Respawn.Graph;

namespace news.feed.Tests.Api;

/// <summary>
/// Helper for reliable and fast database state management in integration/API tests.
/// Uses Respawn to truncate data while keeping the schema.
/// </summary>
public static class TestDatabaseHelper
{
    private static Respawner? _respawner;

    /// <summary>
    /// Resets all mutable data in the test database (news, news_body, news_attachment, etc.).
    /// The "news_program" table is intentionally ignored because it contains static reference data
    /// that is seeded once on application startup.
    /// </summary>
    public static async Task ResetDatabaseAsync(NewsFeedApiFactory factory)
    {
        await using var connection = factory.GetDbConnection();
        await connection.OpenAsync();

        if (_respawner is null)
        {
            _respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = new[] { "public" },
                // We do not want to delete the seeded programs on every test reset.
                TablesToIgnore = new[]
                {
                    new Table("public", "news_program")
                }
            });
        }

        await _respawner.ResetAsync(connection);
    }
}
