using Tools.MarkdownTextExtensions;

namespace Tools.TextProcessors;

public class MarkdownTextProcessor : ITextProcessor
{
    public string ProcessText(string markdown) =>
        markdown.GetParagraphs()
            .Select(paragraph => paragraph.ToHtml())
            .ConcatHtmlCode();
}