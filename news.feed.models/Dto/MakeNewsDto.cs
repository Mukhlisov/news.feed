namespace news.feed.models.Dto;

public class MakeNewsDto
{
    public string Title { get; set; }
    public string Body { get; set; }
    public Program Program { get; set; }

    public List<IncomingAttachment> Attachments { get; set; }
}