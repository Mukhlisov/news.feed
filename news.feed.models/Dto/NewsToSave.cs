namespace news.feed.models.Dto;

public record NewsToSave(
    string Title,
    string Body,
    string PreviewUrl,
    string Program,
    long CreationDate,
    long LastUpdateDate,
    Guid CreatorId);