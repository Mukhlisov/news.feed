using news.feed.models.Dto;
using news.feed.models.Models;

namespace news.feed.Utilities;

public static class NewsFactory
{
    public static NewsToSave Create(CreateNewsDto createNewsDto, Guid creatorId)
    {
        return new NewsToSave(
            createNewsDto.Title,
            createNewsDto.Body,
            createNewsDto.PreviewUrl,
            createNewsDto.Program,
            DateTime.UtcNow.Ticks,
            DateTime.UtcNow.Ticks,
            creatorId);
    }

    public static News Create(News oldNews, UpdateNewsDto updateNewsDto)
    {
        return new News
        {
            Id = oldNews.Id,
            Title = updateNewsDto.Title,
            Program = oldNews.Program,
            PreviewUrl = updateNewsDto.PreviewUrl,
            BodyId = oldNews.BodyId,
            CreationTime = oldNews.CreationTime,
            UpdateTime = DateTime.UtcNow.Ticks,
            AuthorId = oldNews.AuthorId
        };
    }
}