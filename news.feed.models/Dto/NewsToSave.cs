namespace news.feed.models.Dto;

public class NewsToSave
{
    public string Title { get; }
    public string Body { get; }
    public string Program { get; }
    public long CreationDate { get; }
    public long LastUpdateDate { get; }
    public Guid CreatorId { get; }

    public NewsToSave(string title, string body, string program, long creationDate, long lastUpdateDate, Guid creatorId)
    {
        Title = title;
        Body = body;
        Program = program;
        CreationDate = creationDate;
        LastUpdateDate = lastUpdateDate;
        CreatorId = creatorId;
    }
}