using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using news.feed.Services.Auth;

namespace news.feed.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthAttribute : Attribute, IAsyncActionFilter
{
    private const string HeaderName = "X-Babywalk-Token";

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var providedToken = context.HttpContext.Request.Headers[HeaderName].FirstOrDefault();
        var sessionManager = context.HttpContext.RequestServices.GetRequiredService<ISessionManager>();

        if (!sessionManager.VerifySessionToken(providedToken))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        await next();
    }
}