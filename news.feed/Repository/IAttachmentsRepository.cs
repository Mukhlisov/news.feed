namespace news.feed.Repository;

public interface IAttachmentsRepository
{
    Task BatchCreateAttachmentsAsync(List<string> attachmentUris, Guid newsBodyId);
}