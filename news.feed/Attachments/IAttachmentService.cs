using news.feed.models;

namespace news.feed.Attachments;

public interface IAttachmentService
{
    public Task<List<AttachmentToSave>> SaveAttachments(List<Attachment> attachments);
}