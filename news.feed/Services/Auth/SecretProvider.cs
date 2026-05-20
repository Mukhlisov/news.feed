using System.Security.Cryptography;
using extra;
using Microsoft.Extensions.Caching.Memory;
using news.feed.models.Models.Secret;

namespace news.feed.Services.Auth;

public class SecretProvider : ISecretProvider
{
    private readonly IMemoryCache _memoryCache;

    public SecretProvider(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public Secret<string> CreateSecretFor(string issuer)
    {
        var secret = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        _memoryCache.Set(issuer, secret, 28.Days());
        return new Secret<string>(secret, SecretStatus.Created);
    }

    public Secret<string> GetSecret(string issuer)
    {
        if (_memoryCache.TryGetValue(issuer, out string? secret) && secret is not null)
            return new Secret<string>(secret, SecretStatus.Found);
        return new Secret<string>(null, SecretStatus.Expired);
    }
}