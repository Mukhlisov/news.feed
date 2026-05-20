using news.feed.models.Dto.Auth;

namespace news.feed.Services.Auth;

public interface IAuthenticationService
{ 
    void CheckIsAuthenticated(LoginDto loginDto);
}