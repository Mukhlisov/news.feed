using System.Data.Common;

namespace news.feed.models.Exceptions;

public class FailToModifyDataException : DbException
{
    public FailToModifyDataException(string message) : base(message) { }
}