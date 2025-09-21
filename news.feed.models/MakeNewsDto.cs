namespace news.feed.models;

public class MakeNewsDto
{
    public string Title { get; set; }
    public string Body { get; set; }
    public Program Program { get; set; }

    public List<Attachment> Attachments { get; set; }
}