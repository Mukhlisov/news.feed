using System.ComponentModel.DataAnnotations;

namespace news.feed.models.Dto;

public class CreateNewsDto
{
    [MinLength(1), MaxLength(Consts.MaxNewsTitleLength)]
    public string Title { get; set; }
    public string Body { get; set; }
    public string Program { get; set; }
}