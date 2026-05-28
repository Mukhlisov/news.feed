using System.ComponentModel.DataAnnotations;
using news.feed.models.Validation;

namespace news.feed.models.Dto;

public record UpdateNewsDto(
    Guid Id,
    [MaxLength(Consts.MaxNewsTitleLength)]
    [NotWhiteSpace]
    string Title, 
    string PreviewUrl,
    string Body,
    List<AttachmentsDto> Attachments);