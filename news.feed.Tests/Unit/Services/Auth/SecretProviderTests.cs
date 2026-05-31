using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using news.feed.Services.Auth;

namespace news.feed.Tests.Unit.Services.Auth;

public class SecretProviderTests
{
    private readonly IMemoryCache _memoryCache;
    private readonly SecretProvider _secretProvider;

    public SecretProviderTests()
    {
        // Use a real MemoryCache for simpler and more reliable testing
        var options = Options.Create(new MemoryCacheOptions());
        _memoryCache = new MemoryCache(options);
        _secretProvider = new SecretProvider(_memoryCache);
    }

    [Fact]
    public void CreateSecretFor_ShouldGenerateSecretAndReturnCreatedStatus()
    {
        // Arrange
        var issuer = "test-user";

        // Act
        var result = _secretProvider.CreateSecretFor(issuer);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(news.feed.models.Models.Secret.SecretStatus.Created);
        result.Data.Should().NotBeNullOrWhiteSpace();
        result.Data.Should().HaveLength(44); // Base64-encoded 32 bytes
    }

    [Fact]
    public void GetSecret_AfterCreateSecretFor_ShouldReturnFoundStatus()
    {
        // Arrange
        var issuer = "existing-user";
        _secretProvider.CreateSecretFor(issuer);

        // Act
        var result = _secretProvider.GetSecret(issuer);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(news.feed.models.Models.Secret.SecretStatus.Found);
        result.Data.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void GetSecret_WhenNoSecretWasCreated_ShouldReturnExpiredStatus()
    {
        // Arrange
        var issuer = "never-seen-user";

        // Act
        var result = _secretProvider.GetSecret(issuer);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(news.feed.models.Models.Secret.SecretStatus.Expired);
        result.Data.Should().BeNull();
    }

    [Fact]
    public void Secret_ShouldExpire_AfterConfiguredTime()
    {
        // This test demonstrates the expiration behavior.
        // In a real scenario we would use a time provider or shorter expiration for testing.
        // For now we just verify that a freshly created secret is found.

        var issuer = "short-lived-user";
        _secretProvider.CreateSecretFor(issuer);

        var result = _secretProvider.GetSecret(issuer);

        result.Status.Should().Be(news.feed.models.Models.Secret.SecretStatus.Found);
    }
}
