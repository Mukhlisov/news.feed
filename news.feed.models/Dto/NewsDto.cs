namespace news.feed.models.Dto;

public class NewsDto
{
    public Guid Id { get; set; }
    public string Program { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string PreviewUrl { get; set; } = string.Empty;
    public long CreationTime { get; set; }
    public long UpdateTime { get; set; }
    public Guid AuthorId { get; set; }

    public string Body { get; set; } = string.Empty;
    public List<string> AttachmentsUris { get; set; } = new();
}
