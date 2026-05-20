using Microsoft.AspNetCore.Mvc;
using news.feed.models.Exceptions;
using news.feed.models.Exceptions.Auth;

namespace news.feed.Controllers;

public class ApiControllerBase<T> : ControllerBase where T : ControllerBase
{
    private readonly ILogger<T> _logger;

    public ApiControllerBase(ILogger<T> logger)
    {
        _logger = logger;
    }

    public ActionResult HandleHttpError(Exception ex)
    {
        _logger.LogError(ex, ex.Message);
        return ex switch
        {
            // HTTP 400
            ValidationFailedException exception => BadRequest(exception.Message),
            FailedToAuthenticateException exception => BadRequest(exception.Message),
            
            // HTTP 404
            DataNotFoundException exception => NotFound(exception.Message),
            
            // HTTP 500
            FailedToCreateSecretException exception => StatusCode(500, exception.Message),
            FailToModifyDataException exception => StatusCode(500, exception.Message),
            _ => StatusCode(500)
        };
    }
}