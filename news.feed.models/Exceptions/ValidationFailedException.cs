namespace news.feed.models.Exceptions;

public class ValidationFailedException : ArgumentException
{
    public ValidationFailedException(string message) : base(message) { }
}