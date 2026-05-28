using System.ComponentModel.DataAnnotations;
using news.feed.models.Validation;

namespace news.feed.models.Dto;

public record CreateNewsDto(
    [MaxLength(Consts.MaxNewsTitleLength)]
    [NotWhiteSpace]
    string Title, 
    string PreviewUrl,
    string Body, 
    string Program,
    List<AttachmentsDto> AttachmentUris);