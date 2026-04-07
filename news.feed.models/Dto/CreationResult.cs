namespace news.feed.models.Dto;

public class CreationResult<T>
{
    public Uri Uri { get; set; }
    public T Result { get; set; }
}