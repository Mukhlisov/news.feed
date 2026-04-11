using Microsoft.AspNetCore.Mvc;
using news.feed.models;
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
    public async Task<ActionResult<IEnumerable<News>>> GetNews(
        [FromQuery(Name = "skip"), ValueRangeCheck(0, Consts.MaxSkip)] int skip,
        [FromQuery(Name = "take"), ValueRangeCheck(0, Consts.DefaultNewsBatchSize)] int take)
    {
        try
        {
            var news = await _newsService.BatchGetNewsAsync(skip, take).ConfigureAwait(false);
            return Ok(news);
        }
        catch (Exception ex)
        {
            return HandleHttpError(ex);
        }
    }

    [HttpGet("{program}")]
    public async Task<ActionResult<IEnumerable<News>>> GetNewsFromSpecifiedProgram(
        [FromRoute(Name = "program"), ProgramValidation] string program,
        [FromQuery(Name = "skip"), ValueRangeCheck(0, Consts.MaxSkip)] int skip,
        [FromQuery(Name = "take"), ValueRangeCheck(0, Consts.DefaultNewsBatchSize)] int take)
    {
        try
        {
            var news = await _newsService.BatchGetNewsFromSpecifiedProgramAsync(program, skip, take)
                .ConfigureAwait(false);
            return Ok(news);
        }
        catch (Exception ex)
        {
            return HandleHttpError(ex);
        }
    }

    [HttpGet("body/{id:guid}")]
    public async Task<ActionResult<NewsBody>> GetNewsBodyById(Guid id)
    {
        try
        {
            var newsBody = await _newsService.GetNewsBodyByIdAsync(id).ConfigureAwait(false);
            return Ok(newsBody);
        }
        catch (Exception ex)
        {
            return HandleHttpError(ex);
        }
    }

    // TODO AddAuth
    [HttpPost]
    public async Task<ActionResult> CreateNews([FromBody, ProgramValidation] CreateNewsDto createNewsDto)
    {
        try
        {
            var creationResult = await _newsService.CreateNewsAsync(createNewsDto).ConfigureAwait(false);
            return Created(creationResult.Uri, creationResult.Result);
        }
        catch (Exception ex)
        {
            return HandleHttpError(ex);
        }
    }

    // TODO AddAuth
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
            return HandleHttpError(ex);
        }
    }

    // TODO AddAuth
    [HttpPatch]
    public async Task<ActionResult> UpdateNews([FromBody] UpdateNewsDto updateNewsDto)
    {
        try
        {
            var updateResult = await _newsService.UpdateNewsAsync(updateNewsDto).ConfigureAwait(false);
            return Created(updateResult.Uri, updateResult.Result);
        }
        catch (Exception ex)
        {
            return HandleHttpError(ex);
        }
    }
    // TODO search by news title (fuzzy match)
}