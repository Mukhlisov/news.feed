using Microsoft.AspNetCore.Mvc;
using news.feed.models.Exceptions;

namespace news.feed.Controllers;

public class ApiControllerBase<T> : ControllerBase
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
            FailToModifyDataException exception => StatusCode(500, exception.Message),
            ValidationFailedException exception => BadRequest(exception.Message),
            DataNotFoundException exception => NotFound(exception.Message),
            _ => StatusCode(500)
        };
    }
}