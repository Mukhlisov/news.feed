using Microsoft.AspNetCore.Mvc;
using news.feed.models;

namespace news.feed.Controllers;

[ApiController]
[Route("[controller]")]
public class NewsController : ControllerBase
{
    [HttpGet]
    public object GetNews(int skip, int take)
    {
        return NotFound("No such page");
    }

    [HttpPost("program/{program}/create")]
    public void MakeNews([FromBody] MakeNewsDto model, int program)
    {
        
    }
}