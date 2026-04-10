using System.ComponentModel.DataAnnotations;

namespace news.feed.models.Dto;

public record CreateNewsDto(
    [MinLength(1), MaxLength(Consts.MaxNewsTitleLength)]
    string Title, 
    string Body, 
    string Program);