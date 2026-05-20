namespace news.feed.models.Exceptions.Auth;

public class FailedToAuthenticateException : Exception
{
    public FailedToAuthenticateException(string message) : base(message) { }
}