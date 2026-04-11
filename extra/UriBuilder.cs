namespace extra;

public class UriBuilder
{
    private const string HttpProtocol = "http";
    private const string HttpsProtocol = "https";

    private readonly List<string> _segments = new();

    private readonly string _domain;

    public UriBuilder(string domain)
    {
        _domain = domain;
    }

    public UriBuilder AppendSegment<T>(T? segment)
    {
        var value = segment?.ToString();
        ArgumentNullException.ThrowIfNull(value);
        _segments.Add(value);
        return this;
    }

    public Uri BuildHttp()
    {
        var url = BuildLink(HttpProtocol);
        return new Uri(url);
    }

    public Uri BuildHttps()
    {
        var url = BuildLink(HttpsProtocol);
        return new Uri(url);
    }

    private string BuildLink(string protocol) => $"{protocol}://{_domain}/{string.Join("/", _segments)}";
}