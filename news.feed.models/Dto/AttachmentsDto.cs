namespace news.feed.models.Dto;

// При отсутствии Guid считаем, что это новое вложение, которое надо сохранить
// Оставшаяся группа вложений остается в покое
public record AttachmentsDto(Guid? Id, string AttachmentUrl);