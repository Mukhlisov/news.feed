using news.feed.Attachments;
using news.feed.models;
using Tools;

namespace news.feed.News;

public class NewsService(
    IAttachmentService attachmentService,
    INewsRepository newsRepository,
    IAttachmentRepository attachmentRepository) : INewsService
{
    private readonly IAttachmentService _attachmentService = attachmentService;
    private readonly INewsRepository _newsRepository = newsRepository;
    private readonly IAttachmentRepository _attachmentRepository = attachmentRepository;

    public async Task SaveNews(MakeNewsDto makeNewsDto)
    {
        var saveAttachmentsTask = _attachmentService.SaveAttachments(makeNewsDto.Attachments).ConfigureAwait(false);

        var newsToSave = NewsToSaveFactory.Create(makeNewsDto, Guid.Empty);
        var newsId = await _newsRepository.SaveNews(newsToSave).ConfigureAwait(false);
        var attachments = await saveAttachmentsTask;
        await _attachmentRepository.SaveAttachments(attachments).ConfigureAwait(false);
    }
}