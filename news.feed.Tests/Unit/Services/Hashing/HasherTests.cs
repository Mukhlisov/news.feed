using FluentAssertions;
using news.feed.Services.Hashing;

namespace news.feed.Tests.Unit.Services.Hashing;

public class HasherTests
{
    private readonly Hasher _hasher = new();

    [Fact]
    public void Hash_ShouldReturnNonEmptyString()
    {
        // Arrange
        var password = "MySuperSecretPassword123!";

        // Act
        var hash = _hasher.Hash(password);

        // Assert
        hash.Should().NotBeNullOrWhiteSpace();
        hash.Should().StartWith("$2a$"); // BCrypt prefix
    }

    [Fact]
    public void Hash_SamePasswordTwice_ShouldProduceDifferentHashes()
    {
        // Arrange
        var password = "SamePasswordForBoth";

        // Act
        var hash1 = _hasher.Hash(password);
        var hash2 = _hasher.Hash(password);

        // Assert
        hash1.Should().NotBe(hash2, "BCrypt should use random salt each time");
    }

    [Fact]
    public void Verify_CorrectPassword_ShouldReturnTrue()
    {
        // Arrange
        var password = "CorrectPassword123";
        var hash = _hasher.Hash(password);

        // Act
        var result = _hasher.Verify(password, hash);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Verify_WrongPassword_ShouldReturnFalse()
    {
        // Arrange
        var correctPassword = "CorrectPassword123";
        var wrongPassword = "WrongPassword456";
        var hash = _hasher.Hash(correctPassword);

        // Act
        var result = _hasher.Verify(wrongPassword, hash);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Verify_EmptyPasswordAgainstHash_ShouldReturnFalse()
    {
        // Arrange
        var password = "SomePassword";
        var hash = _hasher.Hash(password);

        // Act
        var result = _hasher.Verify("", hash);

        // Assert
        result.Should().BeFalse();
    }
}
