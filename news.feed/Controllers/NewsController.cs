using Microsoft.AspNetCore.Mvc;
using news.feed.models.Dto;
using news.feed.News;

namespace news.feed.Controllers;

[ApiController]
[Route("[controller]")]
public class NewsController(
    INewsService newsService) : ControllerBase
{
    private readonly INewsService _newsService = newsService;

    [HttpGet]
    public object GetNews(int skip, int take)
    {
        return NotFound("");
    }

    [HttpPost("create")]
    public async void CreateNews([FromBody] MakeNewsDto makeNewsDto)
    {
        // Validate some fields (Program, Attachments (file, size))
        await _newsService.SaveNews(makeNewsDto).ConfigureAwait(false);
    }
}