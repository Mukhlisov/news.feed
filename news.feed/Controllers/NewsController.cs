using Microsoft.AspNetCore.Mvc;
using news.feed.models.Dto;
using news.feed.models.Models;
using news.feed.Services;

namespace news.feed.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController : ApiControllerBase
{
    private readonly INewsService _newsService;
    private readonly ILogger<NewsController> _logger;

    public NewsController(INewsService newsService,  ILogger<NewsController> logger)
    {
        _newsService = newsService;
        _logger = logger;
    }

    [HttpGet]
    public object GetNews(int skip, int take)
    {
        return NotFound("");
    }

    [HttpGet("program/{program}")]
    public ActionResult<IEnumerable<News>> GetNewsFromSpecifiedProgram(
        string program, 
        [FromQuery(Name = "skip")] int skip, 
        [FromQuery(Name = "take")] int take)
    {
        try
        {
            var news = _newsService.GetBatchNewsFromSpecifiedProgram(program, skip, take);
            return Ok(news);
        }
        catch (Exception ex)
        {
            return HandleHttpError(ex);
        }
    }

    [HttpPost("create")]
    public async Task<ActionResult> CreateNews([FromBody] SaveNewsDto saveNewsDto)
    {
        try
        {
            await _newsService.SaveNews(saveNewsDto).ConfigureAwait(false);
            return Created("", null); // (mukhlisov) может потом возвращать url на новость с сайта
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return HandleHttpError(ex);
        }
    }
}