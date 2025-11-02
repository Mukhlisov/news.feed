using news.feed.Attachments;
using news.feed.Config.EntityFramework;
using news.feed.models.Dto;
using Tools;

namespace news.feed.News;

public class NewsService(/*
    IAttachmentService attachmentService,
    INewsRepository newsRepository,
    IAttachmentRepository attachmentRepository*/
    NewsFeedContext dbContext) : INewsService
{
    /*private readonly IAttachmentService _attachmentService = attachmentService;
    private readonly INewsRepository _newsRepository = newsRepository;
    private readonly IAttachmentRepository _attachmentRepository = attachmentRepository;*/

    public async Task SaveNews(MakeNewsDto makeNewsDto)
    {
        //TODO: сделать Guid CreatorId.
        /*var newsToSave = NewsToSaveFactory.Create(makeNewsDto, Guid.Empty);
        var newsId = await _newsRepository.SaveNews(newsToSave).ConfigureAwait(false);
        var attachments = await _attachmentService.SaveAttachments(makeNewsDto.Attachments).ConfigureAwait(false); // wtf?? saving attachments two times?
        await _attachmentRepository.SaveAttachments(attachments, newsId).ConfigureAwait(false);*/
    }
}