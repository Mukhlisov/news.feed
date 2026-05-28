using Microsoft.EntityFrameworkCore;
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

    public async Task BatchCreateAttachmentsAsync(IEnumerable<string> attachmentUris, Guid newsBodyId)
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

    public async Task BatchDeleteAttachmentsAsync(IEnumerable<Guid> attachmentIds)
    {
        try
        {
            _newsFeedContext.Attachments.RemoveRange(attachmentIds.Select(id => new Attachment {Id = id}));
            await _newsFeedContext.SaveChangesAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new FailToModifyDataException(ex.Message);
        }
    }

    public async Task DeleteAttachmentByIdAsync(Guid id)
    {
        try
        {
            var attachment = await _newsFeedContext.Attachments.FirstOrDefaultAsync(attachment => attachment.Id == id)
                .ConfigureAwait(false);
            if (attachment == null)
                throw new DataNotFoundException($"Attachment with id {id} not found");
            _newsFeedContext.Attachments.Remove(attachment);
            await _newsFeedContext.SaveChangesAsync().ConfigureAwait(false);
        }
        catch (DataNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new FailToModifyDataException(ex.Message);
        }
    }
}