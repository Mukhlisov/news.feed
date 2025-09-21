using Tools.MarkdownTextExtensions;

namespace Tools.TextProcessors;

public static class MarkdownTextProcessor
{
    public static string ProcessText(string markdown) =>
        markdown.GetParagraphs()
            .Select(paragraph => paragraph.ToHtml())
            .ConcatHtmlCode();
}