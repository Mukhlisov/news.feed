using Microsoft.AspNetCore.Mvc;
using news.feed.models.Exceptions;

namespace news.feed.Controllers;

public class ApiControllerBase<T> : ControllerBase
{
    protected readonly ILogger<T> Logger;

    public ApiControllerBase(ILogger<T> logger)
    {
        Logger = logger;
    }

    public ActionResult HandleHttpError(Exception ex)
    {
        Logger.LogError(ex, ex.Message);
        return ex switch
        {
            FailToModifyDataException exception => StatusCode(500, exception.Message),
            ValidationFailedException exception => BadRequest(exception.Message),
            DataNotFoundException exception => NotFound(exception.Message),
            _ => StatusCode(500)
        };
    }
}