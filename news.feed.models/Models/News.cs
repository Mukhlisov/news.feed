using news.feed.models.Dto;

namespace news.feed.models.Models;

public class News
{
    public Guid Id { get; set; }
    public Program Program { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public long CreationTime { get; set; }
    public long UpdateTime { get; set; }
    public Guid AuthorId { get; set; }
    public virtual ICollection<Attachment> Attachments { get; set; }
}