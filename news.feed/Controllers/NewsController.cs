using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using news.feed.Auth;
using news.feed.models;
using news.feed.models.Dto;
using news.feed.models.Models;
using news.feed.models.Policies;
using news.feed.Services.News;
using news.feed.Utilities.Attributes;

namespace news.feed.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class NewsController : ControllerBase
{
    private readonly INewsService _newsService;

    public NewsController(INewsService newsService)
    {
        _newsService = newsService;
    }

    [EnableCors(nameof(Policies.GetNewsPolicy))]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<News>>> GetNews(
        [FromQuery(Name = "skip"), ValueRangeCheck(0, Consts.MaxSkip)] int skip,
        [FromQuery(Name = "take"), ValueRangeCheck(0, Consts.DefaultNewsBatchSize)] int take)
    {
        var news = await _newsService.BatchGetNewsAsync(skip, take).ConfigureAwait(false);
        return Ok(news);
    }

    [EnableCors(nameof(Policies.GetNewsPolicy))]
    [HttpGet("{program}")]
    public async Task<ActionResult<IEnumerable<News>>> GetNewsFromSpecifiedProgram(
        [FromRoute(Name = "program"), ProgramValidation] string program,
        [FromQuery(Name = "skip"), ValueRangeCheck(0, Consts.MaxSkip)] int skip,
        [FromQuery(Name = "take"), ValueRangeCheck(0, Consts.DefaultNewsBatchSize)] int take)
    {
        var news = await _newsService.BatchGetNewsFromSpecifiedProgramAsync(program, skip, take)
            .ConfigureAwait(false);
        return Ok(news);
    }

    [EnableCors(nameof(Policies.GetNewsPolicy))]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<NewsDto>> GetNewsById(Guid id)
    {
        var newsDto = await _newsService.GetNewsByIdAsync(id).ConfigureAwait(false);
        return Ok(newsDto);
    }

    [EnableCors(nameof(Policies.GetNewsPolicy))]
    [HttpGet("body/{id:guid}")]
    public async Task<ActionResult<NewsBody>> GetNewsBodyById(Guid id)
    {
        var newsBody = await _newsService.GetNewsBodyByIdAsync(id).ConfigureAwait(false);
        return Ok(newsBody);
    }

    [Auth]
    [EnableCors(nameof(Policies.AdminPanelPolicy))]
    [HttpPost]
    public async Task<ActionResult> CreateNews([FromBody, ProgramValidation] CreateNewsDto createNewsDto)
    {
        var creationResult = await _newsService.CreateNewsAsync(createNewsDto).ConfigureAwait(false);
        return Created(creationResult.Uri, creationResult.Result);
    }

    [Auth]
    [EnableCors(nameof(Policies.AdminPanelPolicy))]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteNews(Guid id)
    {
        await _newsService.DeleteNewsAsync(id).ConfigureAwait(false);
        return NoContent();
    }

    [Auth]
    [EnableCors(nameof(Policies.AdminPanelPolicy))]
    [HttpPatch]
    public async Task<ActionResult> UpdateNews([FromBody] UpdateNewsDto updateNewsDto)
    {
        var updateResult = await _newsService.UpdateNewsAsync(updateNewsDto).ConfigureAwait(false);
        return Created(updateResult.Uri, updateResult.Result);
    }

    [Auth]
    [EnableCors(nameof(Policies.AdminPanelPolicy))]
    [HttpPatch("change-program")]
    public async Task<ActionResult> ChangeProgram([ProgramValidation] ChangeNewsProgramDto changeNewsProgramDto)
    {
        var changeResult = await _newsService.ChangeNewsProgramAsync(changeNewsProgramDto).ConfigureAwait(false);
        return Created(changeResult.Uri, changeResult.Result);
    }
    // TODO search by news title (fuzzy match)
}