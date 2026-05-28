namespace news.feed.models.Models;

public class NewsBody
{
    public Guid Id { get; set; }
    public string Body { get; set; }
    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
}