namespace news.feed.Repository;

public interface IAttachmentsRepository
{
    Task BatchCreateAttachmentsAsync(IEnumerable<string> attachmentUris, Guid newsBodyId);
    Task BatchDeleteAttachmentsAsync(IEnumerable<Guid> attachmentIds);
    Task DeleteAttachmentByIdAsync(Guid id);
}