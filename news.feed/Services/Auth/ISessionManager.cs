namespace news.feed.Services.Auth;

public interface ISessionManager
{
    string CreateSessionToken(string login);
    bool VerifySessionToken(string? token);
}