namespace news.feed.models.Models;

public class Attachment
{
    public Guid Id { get; set; }
    public Guid NewsBodyId { get; set; }
    public string AttachmentUrl { get; set; }
}