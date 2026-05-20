using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using news.feed.models.Dto.Auth;
using news.feed.models.Policies;
using news.feed.Services.Auth;

namespace news.feed.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ApiControllerBase<AuthController>
{
    private readonly ISessionManager _sessionManager;
    private readonly IAuthenticationService _authenticationService;

    public AuthController(
        ISessionManager sessionManager,
        IAuthenticationService authenticationService,
        ILogger<AuthController> logger)
        : base(logger)
    {
        _sessionManager = sessionManager;
        _authenticationService = authenticationService;
    }

    [EnableRateLimiting(nameof(Policies.LoginFixedWindowPolicy))]
    [EnableCors(nameof(Policies.AdminPanelPolicy))]
    [HttpPost]
    public IActionResult Login([FromBody] LoginDto loginDto)
    {
        try
        {
            _authenticationService.CheckIsAuthenticated(loginDto);
            var sessionToken = _sessionManager.CreateSessionToken(loginDto.Login);
            return Ok(new {XBabywalkToken = sessionToken});
        }
        catch (Exception ex)
        {
            return HandleHttpError(ex);
        }
    }
}