using news.feed.models.Dto;

namespace news.feed.Attachments;

public interface IAttachmentService
{
    public Task<List<AttachmentToSave>> SaveAttachments(List<IncomingAttachment> attachments);
}