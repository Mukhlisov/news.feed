namespace news.feed.models.Models;

public class Attachment
{
    public Guid Id { get; set; }
    public string ContentType { get; set; }
    public string ContentLink { get; set; }
    public long UploadDate { get; set; }
    public bool IsExternal { get; set; }
    public virtual News News { get; set; }
}