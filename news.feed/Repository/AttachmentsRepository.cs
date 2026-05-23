using news.feed.Config.EntityFramework;
using news.feed.models.Exceptions;
using news.feed.models.Models;

namespace news.feed.Repository;

public class AttachmentsRepository : IAttachmentsRepository
{
    private readonly NewsFeedContext _newsFeedContext;

    public AttachmentsRepository(NewsFeedContext newsFeedContext)
    {
        _newsFeedContext = newsFeedContext;
    }

    public async Task BatchCreateAttachmentsAsync(List<string> attachmentUris, Guid newsBodyId)
    {
        try
        {
            await _newsFeedContext.Attachments.AddRangeAsync(attachmentUris.Select(uri =>
                new Attachment{ Id = Guid.NewGuid(), AttachmentUrl = uri, NewsBodyId = newsBodyId}))
                .ConfigureAwait(false);
            await _newsFeedContext.SaveChangesAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new FailToModifyDataException(ex.Message);
        }
    }
}