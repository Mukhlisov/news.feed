using Microsoft.AspNetCore.Mvc;
using news.feed.Config.Settings;
using news.feed.models.Dto;
using news.feed.models.Models;
using news.feed.Services;
using news.feed.Utilities;

namespace news.feed.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class NewsController : ApiControllerBase<NewsController>
{
    private readonly INewsService _newsService;

    public NewsController(INewsService newsService, ILogger<NewsController> logger) : base(logger)
    {
        _newsService = newsService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<News>>> GetNewsFromSpecifiedProgram(
        [FromQuery(Name = "program"), ProgramValidation] string program,
        [FromQuery(Name = "skip")] int skip = 0,
        [FromQuery(Name = "take")] int take = AppSettings.DefaultNewsBatchSize)
    {
        try
        {
            var news = await _newsService.GetBatchNewsFromSpecifiedProgramAsync(program, skip, take)
                .ConfigureAwait(false);
            return Ok(news);
        }
        catch (Exception ex)
        {
            return HandleHttpError(ex);
        }
    }

    [HttpGet("body/{id:guid}")]
    public async Task<ActionResult<NewsBody>> GetNewsBody(Guid id)
    {
        try
        {
            var newsBody = await _newsService.GetNewsBodyAsync(id).ConfigureAwait(false);
            return Ok(newsBody);
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
            await _newsService.SaveNewsAsync(saveNewsDto).ConfigureAwait(false);
            return Created("", null); // (mukhlisov) может потом возвращать url на новость с сайта
        }
        catch (Exception ex)
        {
            return HandleHttpError(ex);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteNews(Guid id)
    {
        try
        {
            await _newsService.DeleteNewsAsync(id).ConfigureAwait(false);
            return NoContent();
        }
        catch (Exception ex)
        {
            return HandleHttpError(ex); //TODO: (mukhlisov) добавить кастомный ApiOperationResult (HasErrors, Errors, Result) 
        }
    }
}