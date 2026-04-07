using news.feed.models.Dto;

namespace Tools;

public static class NewsToSaveFactory
{
    public static NewsToSave Create(CreateNewsDto createNewsDto, Guid creatorId)
    {
        return new NewsToSave(
            createNewsDto.Title,
            createNewsDto.Body,
            createNewsDto.Program,
            DateTime.UtcNow.Ticks,
            DateTime.UtcNow.Ticks,
            creatorId);
    }
}