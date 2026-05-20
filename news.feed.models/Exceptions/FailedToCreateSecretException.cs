namespace news.feed.models.Exceptions;

public class FailedToCreateSecretException : Exception
{
    public FailedToCreateSecretException(string message) : base(message) { }
}