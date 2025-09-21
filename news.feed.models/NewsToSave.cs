namespace news.feed.models;

public class NewsToSave
{
    public string Title { get; }
    public string Body { get; }
    public Program Program { get; }
    public long CreationDate { get; }
    public Guid CreatorId { get; }

    public NewsToSave(string title, string body, Program program, long creationDate, Guid creatorId)
    {
        Title = title;
        Body = body;
        Program = program;
        CreationDate = creationDate;
        CreatorId = creatorId;
    }
}