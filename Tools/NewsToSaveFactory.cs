using news.feed.models.Dto;

namespace Tools;

public static class NewsToSaveFactory
{
    public static NewsToSave Create(SaveNewsDto saveNewsDto, Guid creatorId)
    {
        return new NewsToSave(
        saveNewsDto.Title,
        saveNewsDto.Body,
        saveNewsDto.Program,
        DateTime.UtcNow.Ticks,
        DateTime.UtcNow.Ticks,
        creatorId);
    }
}