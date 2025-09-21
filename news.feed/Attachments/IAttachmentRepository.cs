using news.feed.models;

namespace news.feed.Attachments;

public interface IAttachmentRepository
{
    public Task<Guid> SaveAttachment(AttachmentToSave attachmentToSave);
    public Task<List<Guid>> SaveAttachments(List<AttachmentToSave> newsToSave);
}