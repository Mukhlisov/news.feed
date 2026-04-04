using Microsoft.AspNetCore.Mvc;
using news.feed.models.Exceptions;

namespace news.feed.Controllers;

public class ApiControllerBase : ControllerBase
{
    public ActionResult HandleHttpError(Exception ex) => ex switch
    {
        FailToModifyDataException exception => StatusCode(500, exception.Message),
        ValidationFailedException exception => BadRequest(exception.Message),
        _ => StatusCode(500)
    };
}