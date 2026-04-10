namespace news.feed.models.Dto;

public record NewsToSave(
    string Title,
    string Body,
    string Program,
    long CreationDate,
    long LastUpdateDate,
    Guid CreatorId);