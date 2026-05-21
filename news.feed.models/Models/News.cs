namespace news.feed.models.Models;

public class News
{
    public Guid Id { get; set; }
    public string Program { get; set; }
    public string Title { get; set; }
    public string PreviewUrl { get; set; }
    public Guid BodyId { get; set; }
    public long CreationTime { get; set; }
    public long UpdateTime { get; set; }
    public Guid AuthorId { get; set; }
}