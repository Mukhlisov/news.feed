using Microsoft.AspNetCore.Mvc;
using news.feed.models.Dto;
using news.feed.Services;

namespace news.feed.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController(
    INewsService newsService) : NewsApiControllerBase
{
    private readonly INewsService _newsService = newsService;

    [HttpGet]
    public object GetNews(int skip, int take)
    {
        return NotFound("");
    }

    [HttpGet("program/{program}")]
    public ActionResult<string> GetNewsFromSpecifiedProgram(string program, [FromQuery(Name = "skip")] int skip, [FromQuery(Name = "take")] int take)
    {
        try
        {
            var json = _newsService.GetBatchNewsFromSpecifiedProgram(program, skip, take);
            return Ok(json);
        }
        catch (Exception ex)
        {
            return HandleHttpError(ex);
        }
    }

    [HttpPost("save")]
    public async Task<ActionResult> CreateNews([FromBody] SaveNewsDto saveNewsDto)
    {
        try
        {
            await _newsService.SaveNews(saveNewsDto).ConfigureAwait(false);
            return Created("", null); // (mukhlisov) может потом возвращать url на новость с сайта
        }
        catch (Exception ex)
        {
            return HandleHttpError(ex);
        }
    }
}