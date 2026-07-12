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

public class NewsFeedApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresContainer;

    public NewsFeedApiFactory()
    {
        _postgresContainer = new PostgreSqlBuilder()
            .WithImage("postgres:17-alpine")
            .WithDatabase("newsfeed_test")
            .WithUsername("test")
            .WithPassword("test")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilCommandIsCompleted("pg_isready"))
            .Build();
    }

    public string ConnectionString { get; private set; } = string.Empty;

    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();

        ConnectionString = _postgresContainer.GetConnectionString();

        Environment.SetEnvironmentVariable("CONNECTION_STRING", ConnectionString);
        Environment.SetEnvironmentVariable("SITE_DOMAIN", "localhost:3000");
        Environment.SetEnvironmentVariable("ADMIN_PANEL_DOMAIN", "localhost:3001");
        Environment.SetEnvironmentVariable("AUTHOR_ID", Guid.NewGuid().ToString());

        Environment.SetEnvironmentVariable("AUTH_ADMIN_NAME", "testadmin");

        var dummyPasswordHash = BCrypt.Net.BCrypt.HashPassword("TestAdminPasswordForTestsOnly!");
        Environment.SetEnvironmentVariable("PASSWORD_HASH", dummyPasswordHash);

        ApplyMigrationsAndFillData();
    }

    private void ApplyMigrationsAndFillData()
    {
        var options = new DbContextOptionsBuilder<NewsFeedContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        using var context = new NewsFeedContext(options);
        context.Database.Migrate();
        if (context.Programs.Any())
            return;
        context.Programs.AddRange(TestData.Programs);
        context.SaveChanges();
    }

    public new async Task DisposeAsync()
    {
        await _postgresContainer.DisposeAsync();
        await base.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            services.AddControllers()
                .AddApplicationPart(typeof(Controllers.NewsController).Assembly);
        });
    }

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

        var token = sessionManager.CreateSessionToken("testadmin");

        client.DefaultRequestHeaders.Add("X-Babywalk-Token", token);
        return client;
    }

    public Task<HttpClient> CreateAuthenticatedClientAsync() => Task.FromResult(CreateAuthenticatedClient());
}
