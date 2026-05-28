using System.Data.Common;
using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Testcontainers.PostgreSql;
using news.feed.Config.EntityFramework;

namespace news.feed.Tests.Api;

/// <summary>
/// Custom WebApplicationFactory that spins up a real PostgreSQL container via Testcontainers
/// and configures the application to use it for integration/API tests.
/// 
/// This is the core infrastructure for all meaningful automated tests.
/// </summary>
public class NewsFeedApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresContainer;

    public NewsFeedApiFactory()
    {
        _postgresContainer = new PostgreSqlBuilder()
            .WithImage("postgres:16-alpine")
            .WithDatabase("newsfeed_test")
            .WithUsername("test")
            .WithPassword("test")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilCommandIsCompleted("pg_isready"))
            .Build();
    }

    /// <summary>
    /// The connection string to the test Postgres instance.
    /// Valid only after InitializeAsync has completed.
    /// </summary>
    public string ConnectionString { get; private set; } = string.Empty;

    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();

        ConnectionString = _postgresContainer.GetConnectionString();

        // === CRITICAL ===
        // We must set the environment variables BEFORE the application host starts building,
        // because news.feed uses a custom static settings system (SettingsInitializer + [Setting] attributes).
        //
        // These values will be picked up by PostgresSettingsStorage, AppSettings, AuthSettings etc.
        Environment.SetEnvironmentVariable("CONNECTION_STRING", ConnectionString);
        Environment.SetEnvironmentVariable("SITE_DOMAIN", "http://localhost:5000");
        Environment.SetEnvironmentVariable("AUTHOR_ID", Guid.NewGuid().ToString());

        // Test admin user for authentication in tests
        Environment.SetEnvironmentVariable("AUTH_ADMIN_NAME", "testadmin");

        // We must provide PASSWORD_HASH because AuthSettings marks it as [Secret] and
        // SettingsInitializer will throw if the env var is missing, even if we bypass
        // password-based login in tests by using SessionManager directly.
        var dummyPasswordHash = BCrypt.Net.BCrypt.HashPassword("TestAdminPasswordForTestsOnly!");
        Environment.SetEnvironmentVariable("PASSWORD_HASH", dummyPasswordHash);

        // === VERY IMPORTANT ===
        // The test database is completely empty (fresh container).
        // The real application calls FillProgramsTableIfNotExists on startup,
        // which queries the "news_program" table.
        // We must apply EF migrations BEFORE the host starts building.
        ApplyMigrations();
    }

    private void ApplyMigrations()
    {
        var options = new DbContextOptionsBuilder<NewsFeedContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        using var context = new NewsFeedContext(options);
        context.Database.Migrate();
    }

    public new async Task DisposeAsync()
    {
        await _postgresContainer.DisposeAsync();
        await base.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // We deliberately do NOT touch the DbContext registration here.
        // The normal registration in ConfigureDbSettings() (via environment variable CONNECTION_STRING)
        // must be allowed to happen.

        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // Explicitly register controllers from the main application assembly.
            // This is required for WebApplicationFactory to discover controllers reliably
            // when using a custom bootstrap (Application.Run + extension methods).
            services.AddControllers()
                .AddApplicationPart(typeof(news.feed.Controllers.NewsController).Assembly);
        });

        // Optional: you can add more test-specific configuration here later
        // builder.ConfigureServices(services => { ... });
    }

    /// <summary>
    /// Returns a raw Npgsql connection to the test database.
    /// Useful for Respawn, direct cleanup, or low-level assertions.
    /// </summary>
    public DbConnection GetDbConnection() => new NpgsqlConnection(ConnectionString);

    /// <summary>
    /// Creates an HttpClient that is already authenticated as the test admin.
    /// This is the preferred way to get an authorized client in tests.
    /// </summary>
    public HttpClient CreateAuthenticatedClient()
    {
        var client = CreateClient();

        using var scope = Services.CreateScope();
        var sessionManager = scope.ServiceProvider.GetRequiredService<news.feed.Services.Auth.ISessionManager>();

        // Uses the real implementation → creates a secret in IMemoryCache and returns a valid token.
        var token = sessionManager.CreateSessionToken("testadmin");

        client.DefaultRequestHeaders.Add("X-Babywalk-Token", token);
        return client;
    }

    public Task<HttpClient> CreateAuthenticatedClientAsync() => Task.FromResult(CreateAuthenticatedClient());
}
