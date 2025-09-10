namespace Tools.MarkdownTextExtensions;

public static class EnumerableExtensions
{
    public static string ConcatHtmlCode(this IEnumerable<string> htmlParagraphs)
    {
        return string.Join(string.Empty, htmlParagraphs);
    }
}