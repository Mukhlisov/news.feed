namespace news.feed.models.Dto;

public class GetNewsDto
{
    public Guid NewsId { get; set; }
    public string Title { get; set; }
    public string Program { get; set; }
    public Guid BodyId { get; set; }
    public long CreationDate { get; set; }
    public long LastUpdateDate { get; set; }

    public GetNewsDto(Guid newsId, string title, string program, Guid bodyId, long creationDate, long lastUpdateDate)
    {
        NewsId = newsId;
        Title = title;
        Program = program;
        BodyId = bodyId;
        CreationDate = creationDate;
        LastUpdateDate = lastUpdateDate;
    }
}