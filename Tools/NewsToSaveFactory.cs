using news.feed.models.Dto;
using Tools.TextProcessors;

namespace Tools;

public static class NewsToSaveFactory
{
    public static NewsToSave Create(MakeNewsDto makeNewsDto, Guid creatorId)
    {
        var html = MarkdownTextProcessor.ProcessText(makeNewsDto.Body);
        return new NewsToSave(
        makeNewsDto.Title,
        html,
        makeNewsDto.Program,
        DateTime.UtcNow.Ticks,
        creatorId);
    }
}