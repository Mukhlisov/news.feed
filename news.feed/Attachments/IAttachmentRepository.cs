using news.feed.models.Dto;

namespace news.feed.Attachments;

public interface IAttachmentRepository
{
    public Task<Guid> SaveAttachment(AttachmentToSave attachmentToSave, Guid newsId);
    public Task<List<Guid>> SaveAttachments(List<AttachmentToSave> newsToSave, Guid newsId);
}