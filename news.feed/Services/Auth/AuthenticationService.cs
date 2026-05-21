using news.feed.Config.Settings;
using news.feed.models.Dto.Auth;
using news.feed.models.Exceptions.Auth;
using news.feed.Services.Hashing;

namespace news.feed.Services.Auth;

public class AuthenticationService : IAuthenticationService
{
    private readonly IHasher _hasher;

    public AuthenticationService(IHasher hasher)
    {
        _hasher = hasher;
    }

    public void CheckIsAuthenticated(LoginDto loginDto)
    {
        if (AuthSettings.AdminName == loginDto.Login && _hasher.Verify(loginDto.Password, AuthSettings.PasswordHash))
            return;
        throw new FailedToAuthenticateException("Incorrect login or password");
    }
}