using System.Text.RegularExpressions;

namespace Tools.MarkdownTextExtensions;

public static class StringExtensions
{
    private const string ParagraphSplitPattern = @"\n(?:\s*\n)+";
    private const string HeadersPattern = @"^(#{1,6})\s*(.+)$";
    private const string BoldTextPattern = @"\*\*(.+?)\*\*|__(.+?)__";
    private const string ItalicTextPattern = @"\*(.+?)\*|_(.+?)_";

    public static IEnumerable<string> GetParagraphs(this string markdown) =>
        Regex.Split(markdown, ParagraphSplitPattern, RegexOptions.IgnoreCase)
            .Select(p => p.Trim())
            .Where(p => !string.IsNullOrEmpty(p));

    public static string ToHtml(this string paragraph)
    {
        if (string.IsNullOrWhiteSpace(paragraph))
            return string.Empty;

        var headerMatch = Regex.Match(paragraph, HeadersPattern, RegexOptions.IgnoreCase);
        if (headerMatch.Success)
        {
            var headerLevel = headerMatch.Groups[1].Value.Length;
            var headerText = headerMatch.Groups[2].Value;

            headerText = Regex.Replace(headerText, BoldTextPattern, m => $"<strong>{DecideBetweenMatchingGroups(m.Groups[1], m.Groups[2])}</strong>");
            headerText = Regex.Replace(headerText, ItalicTextPattern, m => $"<em>{DecideBetweenMatchingGroups(m.Groups[1], m.Groups[2])}</em>");
            
            return $"<h{headerLevel}>{headerText}</h{headerLevel}>";
        }

        var html = paragraph;
        html = Regex.Replace(html, BoldTextPattern, m => $"<strong>{DecideBetweenMatchingGroups(m.Groups[1], m.Groups[2])}</strong>");
        html = Regex.Replace(html, ItalicTextPattern, m => $"<em>{DecideBetweenMatchingGroups(m.Groups[1], m.Groups[2])}</em>");
        html = html.Replace("\n", "</br>");
        return $"<p>{html}</p>";
    }

    private static string DecideBetweenMatchingGroups(params Group[] groups)
    {
        foreach (var group in groups)
            if (group.Success) return group.Value;
        return string.Empty;
    } 
}