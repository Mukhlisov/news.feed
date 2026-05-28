using System.ComponentModel.DataAnnotations;

namespace news.feed.models.Dto;

public record UpdateNewsDto(
    Guid Id,
    [MinLength(1), MaxLength(Consts.MaxNewsTitleLength)]
    string Title, 
    string PreviewUrl,
    string Body,
    List<AttachmentsDto> Attachments);