namespace Tools.MarkdownTextExtensions;

public static class EnumerableExtensions
{
    public static string CombineHtmlCode(this IEnumerable<string> htmlParagraphs)
    {
        return string.Join(Environment.NewLine, htmlParagraphs);
    }
}