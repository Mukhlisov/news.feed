using news.feed.Config.Settings;
using news.feed.Services.Hashing;

namespace news.feed.Services.Auth;

public class SessionManager : ISessionManager
{
    private readonly ISecretProvider _secretProvider;
    private readonly IHasher _hasher;

    public SessionManager(ISecretProvider secretProvider, IHasher hasher)
    {
        _secretProvider = secretProvider;
        _hasher = hasher;
    }

    public string CreateSessionToken(string login)
    {
        var secret = _secretProvider.CreateSecretFor(login);
        secret.EnsureCreated();
        var sessionToken = _hasher.Hash($"{login}--{secret.Data}");
        return sessionToken;
    }

    public bool VerifySessionToken(string? token)
    {
        if (string.IsNullOrEmpty(token))
            return false;

        var secret = _secretProvider.GetSecret(AuthSettings.AdminName);
        return !secret.IsExpired && _hasher.Verify($"{AuthSettings.AdminName}--{secret.Data}", token);
    }
}